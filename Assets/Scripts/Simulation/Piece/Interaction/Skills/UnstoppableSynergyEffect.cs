using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnstoppableSynergyEffect : Interaction
{
    private Piece caster;
    private Board board;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Synergy.KnightTicksOfUnstoppable;

    public UnstoppableSynergyEffect(Piece caster, Board board)
    {
        this.caster = caster;
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
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
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
        caster.invulnerable = false;
    }
}
