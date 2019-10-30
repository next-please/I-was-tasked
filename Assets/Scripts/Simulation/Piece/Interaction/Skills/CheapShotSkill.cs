using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CheapShotSkill : Interaction
{
    public Piece caster;
    private Piece target;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 50;
    public int cheapShotDefaultStunDuration = GameLogicManager.Inst.Data.Skills.CheapShotStunTicks;

    public CheapShotSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.CheapShot;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 2.0f;
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
            ApplyDamageToInflict();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        projectile.transform.position = attackSource;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (target.IsDead())
        {
            return;
        }

        int stunDuration = (int)Math.Floor(cheapShotDefaultStunDuration * Math.Pow(GameLogicManager.Inst.Data.Skills.CheapShotRarityMultiplier, caster.GetRarity()));

        if (target.GetState().ticksRemaining < stunDuration)
        {
            target.GetState().ticksRemaining = stunDuration;
        }

        Interaction skill = new CheapShotLingeringEffect(target, caster);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has CheapShot-ed " + target.GetName() + ", stunning them for " + stunDuration + " ticks.");
    }
}

public class CheapShotLingeringEffect : Interaction
{
    private Vector3 effectPosition;
    private Piece target;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.CheapShotLingerTicks;

    public CheapShotLingeringEffect(Piece target, Piece caster)
    {
        this.target = target;
        if (!target.IsDead())
        {
            effectPosition = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            effectPosition.y = 2f;
        }
        int ticks = (int)Math.Floor(ticksTilActivation * Math.Pow(GameLogicManager.Inst.Data.Skills.CheapShotRarityMultiplier, caster.GetRarity()));
        this.ticksRemaining = ticks;
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
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;
        if (!target.IsDead())
        {
            effectPosition = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            effectPosition.y = 2f;
        }

        projectile.transform.position = effectPosition;

        if (ticksRemaining <= 0 || target.IsDead())
        {
            return false;
        }
        return true;
    }
}
