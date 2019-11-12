﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarkForDeathSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.MarkForDeathTicks;

    public MarkForDeathSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        int ticks = (int)Math.Floor(ticksTilActivation * Math.Pow(GameLogicManager.Inst.Data.Skills.MarkForDeathRarityMultiplier, caster.GetRarity()));
        this.ticksRemaining = ticks;
        this.ticksTotal = 50;
        interactionPrefab = Enums.InteractionPrefab.MarkForDeath;
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
        GameObject projectile = interactionView.gameObject;
        if (!target.IsDead())
        {
            Transform targetT = target.GetPieceView().transform;
            projectile.transform.parent =  targetT;
            Vector3 pos = Vector3.zero;
            pos.y = 1;
            projectile.transform.localPosition = pos;
        }

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        if (target.IsDead() || target.invulnerable == true)
        {
            return;
        }
        target.SetCurrentHitPoints(0);

        Debug.Log(caster.GetName() + " has MarkForDeath-ed " + target.GetName() + ".");
    }
}
