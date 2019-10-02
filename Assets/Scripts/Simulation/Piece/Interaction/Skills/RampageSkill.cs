﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RampageSkill : Interaction
{
    private Piece caster;
    private Board board;
    public int rampageDefaultAttackSpeedAmount = 3;
    public double rampageDefaultArmourPercentage = -0.25;
    public int ticksTilActivation = 0;

    public RampageSkill(Piece caster, Board board)
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

        int attackSpeedChange = caster.SetAttackSpeed(caster.GetAttackSpeed() + rampageDefaultAttackSpeedAmount);
        caster.SetArmourPercentage(caster.GetArmourPercentage() + rampageDefaultArmourPercentage);
        double armourChange = rampageDefaultArmourPercentage;

        Interaction skill = new RampageLingeringEffect(caster, attackSpeedChange, armourChange);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Rampaged-ed " + caster.GetName() + " to increase attack speed to " + caster.GetAttackSpeed() + " and armour to " + caster.GetArmourPercentage() + ".");
    }
}

public class RampageLingeringEffect : Interaction
{
    private Piece caster;
    private int attackSpeedChange;
    private double armourChange;
    private Vector3 attackDestination;
    public int ticksTilActivation = 250;
    public int blockAmount;

    public RampageLingeringEffect(Piece caster, int attackSpeedChange, double armourChange)
    {
        this.caster = caster;
        this.attackSpeedChange = attackSpeedChange;
        this.armourChange = armourChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestBloodRed;
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

        attackDestination = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackDestination.y += 3.5f;

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        if (caster.IsDead())
        {
            return;
        }
        caster.SetAttackSpeed(caster.GetAttackSpeed() - attackSpeedChange);
        caster.SetArmourPercentage(caster.GetArmourPercentage() - armourChange);

        Debug.Log(caster.GetName() + "'s Rampage has expired.");
    }

}