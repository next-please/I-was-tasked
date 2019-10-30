using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvicerateSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int evicerateDefaultInitialDamage = GameLogicManager.Inst.Data.Skills.EviscerateInitialDamage;
    public int evicerateDefaultBleedCount = GameLogicManager.Inst.Data.Skills.EviscerateBleedCount;
    public int evicerateDefaultBleedDamage = GameLogicManager.Inst.Data.Skills.EviscerateBleedDamage;
    public int ticksTilActivation = 0;

    public EvicerateSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            return true;
        }
        else
        {
            ApplyEffect();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    private void ApplyEffect()
    {
        if (caster.IsDead())
        {
            return;
        }

        int damage = (int)Math.Floor(evicerateDefaultInitialDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.EviscerateRarityMultiplier, caster.GetRarity()));
        if (!target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damage);
        }

        int bleedCount = evicerateDefaultBleedCount;
        int bleedDamage = evicerateDefaultBleedDamage;
        bleedDamage = (int)Math.Floor(bleedDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.EviscerateRarityMultiplier, caster.GetRarity()));
        Interaction skill = new EvicerateLingeringEffect(target, bleedDamage, bleedCount, board);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Evicerate-ed " + target.GetName() + " to damage for " + damage + " damage and add a bleed.");
    }
}

public class EvicerateLingeringEffect : Interaction
{
    private Piece target;
    private Board board;
    public int bleedDamage;
    public int countRemaining;
    private Vector3 attackDestination;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.EviscerateLingerTicks;

    public EvicerateLingeringEffect(Piece target, int bleedDamage, int countRemaining, Board board)
    {
        this.target = target;
        this.board = board;
        this.bleedDamage = bleedDamage;
        this.countRemaining = countRemaining;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.EviscerateBleed;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            if (ticksRemaining == 0 && countRemaining > 0)
            {
                ApplyEffect();
                ticksRemaining = ticksTilActivation;
                countRemaining--;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        if (!target.IsDead())
        {
            GameObject projectile = interactionView.gameObject;
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y += 1.5f;
            projectile.transform.position = attackDestination;
        }
        else
        {
            ticksRemaining = 0;
        }

        return (ticksRemaining > 0);
    }

    private void ApplyEffect()
    {
        if (target.IsDead())
        {
            return;
        }

        if (!target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - bleedDamage);
        }
        Debug.Log(target.GetName() + "'s Evicerate has bled for " + bleedDamage + ". " + countRemaining + " bleed ticks left.");
    }

}