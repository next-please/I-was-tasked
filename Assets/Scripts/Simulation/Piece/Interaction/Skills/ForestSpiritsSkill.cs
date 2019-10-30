﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForestSpiritsSkill : Interaction
{
    private Piece caster;
    private Tile targetSpace;
    private Board board;
    private Vector3 attackSource;
    private float attackSourceOffset = 0.5f;
    private int ticksTilActivation = 0;
    private int initialTicksTilActivation = GameLogicManager.Inst.Data.Skills.ForestSpiritsInitialTicks;
    private int subsequentTicksTilActivation = GameLogicManager.Inst.Data.Skills.ForestSpiritsSubsequentTicks;
    public int forestSpiritsDefaultCount = GameLogicManager.Inst.Data.Skills.ForestSpiritsCount;

    public ForestSpiritsSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 1.5f;
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
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        for (int i=0; i<forestSpiritsDefaultCount; i++)
        {
            Interaction skill = new ForestSpiritInitialEffect(caster, attackSource, board, initialTicksTilActivation + (i*subsequentTicksTilActivation));
            board.AddInteractionToProcess(skill);
            attackSource.y += attackSourceOffset;
        }
    }
}

public class ForestSpiritInitialEffect : Interaction
{
    private Vector3 effectSpace;
    private Board board;
    private Piece caster;

    public ForestSpiritInitialEffect(Piece caster, Vector3 effectSpace, Board board, int ticksTilActivation)
    {
        this.caster = caster;
        this.ticksTotal = 50;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.effectSpace = effectSpace;
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
        projectile.transform.position = effectSpace;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (board.GetActiveFriendliesOnBoard().Count == 0 || board.GetActiveEnemiesOnBoard().Count == 0)
            return;

        Piece target;
        List<Piece> damagedFriendlies;

        if (!caster.IsEnemy())
        {
            damagedFriendlies = board.GetActiveFriendliesOnBoard().FindAll(piece => piece.GetCurrentHitPoints() < piece.GetMaximumHitPoints());
        }
        else
        {
            damagedFriendlies = board.GetActiveEnemiesOnBoard().FindAll(piece => piece.GetCurrentHitPoints() < piece.GetMaximumHitPoints());
        }

        if (damagedFriendlies.Count > 0)
        {
            target = damagedFriendlies[board.GetRNGesus().Next(0, damagedFriendlies.Count)];
        }
        else
        {
            target = board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)];
        }

        Interaction skill = new ForestSpiritSecondaryEffect(effectSpace, target);
        board.AddInteractionToProcess(skill);
    }
}

public class ForestSpiritSecondaryEffect : Interaction
{
    private Piece target;
    private Vector3 attackSource;
    private Vector3 attackDestination;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.ForestSpiritsSecondaryTicks;
    public int forestSpiritsDefaultHealAmount = GameLogicManager.Inst.Data.Skills.ForestSpiritsHeal;

    public ForestSpiritSecondaryEffect(Vector3 attackSource, Piece target)
    {
        this.target = target;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ForestSpirits;

        this.attackSource = attackSource;
        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 0.5f;
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

        if (!target.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 0.5f;
        }

        float fracJourney = (float)Math.Pow(((ticksTilActivation - ticksRemaining) / ticksTilActivation), 2);
        projectile.transform.position = Vector3.Lerp(attackSource, attackDestination, fracJourney);
        projectile.transform.LookAt(attackDestination);

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
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() + forestSpiritsDefaultHealAmount);

        Debug.Log(target.GetName() + " has been ForestSpirits-ed for " + forestSpiritsDefaultHealAmount + " healing.");
    }
}
