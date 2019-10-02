﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShapeshiftSkill : Interaction
{
    private Piece caster;
    private Board board;
    public double shapeshiftDefaultMultiplierIncrease = 1.2;
    public int shapeshiftDefaultAttackSpeedIncrease = 1;
    public int ticksTilActivation = 0;

    public ShapeshiftSkill(Piece caster, Board board)
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
        caster.SetAttackDamage((int)Math.Floor(caster.GetAttackDamage() * shapeshiftDefaultMultiplierIncrease));
        caster.SetAttackSpeed(caster.GetAttackSpeed() + shapeshiftDefaultAttackSpeedIncrease);
        caster.SetCurrentHitPoints((int)Math.Floor(caster.GetCurrentHitPoints() * shapeshiftDefaultMultiplierIncrease));
        caster.SetMaximumHitPoints((int)Math.Floor(caster.GetMaximumHitPoints() * shapeshiftDefaultMultiplierIncrease));

        Interaction skill = new ShapeshiftLingeringEffect(caster);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Shapeshift-ed " + caster.GetName() + " to increase health to " + caster.GetCurrentHitPoints() + ", and attack damage to " + caster.GetAttackDamage() + ", and attack speed to " + caster.GetAttackSpeed() + ".");
    }
}

public class ShapeshiftLingeringEffect : Interaction
{
    private Piece caster;
    private Vector3 attackDestination;
    public int ticksTilActivation = 100;

    public ShapeshiftLingeringEffect(Piece caster)
    {
        this.caster = caster;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestRed;
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
            Fizzle();
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

    private void Fizzle()
    {
    }

}