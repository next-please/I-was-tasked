﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvicerateSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int evicerateDefaultInitialDamage = 5;
    public int evicerateDefaultBleedCount = 10;
    public int evicerateDefaultBleedDamage = 1;
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
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - evicerateDefaultInitialDamage);
        int bleedCount = evicerateDefaultBleedCount;
        int bleedDamage = evicerateDefaultBleedDamage;
        Interaction skill = new EvicerateLingeringEffect(target, bleedDamage, bleedCount, board);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Evicerate-ed " + target.GetName() + " to damage for " + evicerateDefaultInitialDamage + " damage and add a bleed.");
    }
}

public class EvicerateLingeringEffect : Interaction
{
    private Piece target;
    private Board board;
    public int bleedDamage;
    public int countRemaining;
    private Vector3 attackDestination;
    public int ticksTilActivation = 30;

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

        return (ticksRemaining > 0);
    }

    private void ApplyEffect()
    {
        if (target.IsDead())
        {
            return;
        }
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - bleedDamage);
        Debug.Log(target.GetName() + "'s Evicerate has bled for " + bleedDamage + ". " + countRemaining + " bleed ticks left.");
    }

}