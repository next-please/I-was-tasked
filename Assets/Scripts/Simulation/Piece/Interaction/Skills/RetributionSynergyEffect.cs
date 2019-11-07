﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RetributionSynergyEffect : Interaction
{
    private Piece caster;
    private Board board;
    private Tile lastKnownTile;
    private int friendlyHealing;
    private int enemyDamage;
    private int radius;
    public int ticksTilActivation = 0;

    public RetributionSynergyEffect(Piece caster, Board board, int r, int e, int f)
    {
        this.identifier = Enums.Interaction.RetributionSynergy;
        this.caster = caster;
        this.lastKnownTile = caster.GetCurrentTile();
        this.board = board;
        this.friendlyHealing = f;
        this.enemyDamage = e;
        this.radius = r;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
    }

    public override bool ProcessInteraction()
    {
        if (!caster.IsDead())
        {
            lastKnownTile = caster.GetCurrentTile();
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
        //interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    public void ApplyEffect()
    {
        foreach (Piece target in board.GetActiveEnemiesWithinRadiusOfTile(lastKnownTile, radius))
        {
            if (!target.invulnerable)
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() - enemyDamage);
                Debug.Log("Retribution from " + caster.GetName() + " damaged " + target.GetName() + " for " + enemyDamage + ".");
            }
        }
        foreach (Piece target in board.GetActiveFriendliesWithinRadiusOfTile(lastKnownTile, radius))
        {
            if (!target.invulnerable)
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() + friendlyHealing);
                Debug.Log("Retribution from " + caster.GetName() + " healed " + target.GetName() + " for " + friendlyHealing + ".");
            }
        }

        board.AddInteractionToProcess(new RetributionLingeringEffect(lastKnownTile));
    }
}
public class RetributionLingeringEffect : Interaction
{
    private Vector3 attackDestination;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.BlessingOfNatureLingerTicks;

    public RetributionLingeringEffect(Tile target)
    {
        this.attackDestination = ViewManager.CalculateTileWorldPosition(target);
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.DivineJudgement;
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
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }
}
