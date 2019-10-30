using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BerserkSkill : Interaction
{
    private Piece caster;
    private Board board;
    public double berserkDefaultMultiplierIncrease = GameLogicManager.Inst.Data.Skills.BerserkMultiplerIncrease;
    public int ticksTilActivation = 0;

    public BerserkSkill(Piece caster, Board board)
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
        caster.SetAttackDamage((int)Math.Floor(caster.GetAttackDamage() * berserkDefaultMultiplierIncrease));

        Debug.Log(caster.GetName() + " has Berserk-ed " + caster.GetName() + " to increase attack damage to " + caster.GetAttackDamage() + ".");
    }
}
