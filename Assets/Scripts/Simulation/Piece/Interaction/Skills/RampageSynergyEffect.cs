using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RampageSynergyEffect : Interaction
{
    private Piece caster;
    private Board board;
    public int rampageDefaultAttackSpeedAmount = GameLogicManager.Inst.Data.Synergy.OrcRampageAttackSpeed;
    public double rampageDefaultArmourPercentage = GameLogicManager.Inst.Data.Synergy.OrcRampageArmourPercentage;
    public double rampageSynergyHealthThreshold = GameLogicManager.Inst.Data.Synergy.OrcRampageHealthThreshold;
    public int ticksTilActivation = 999;

    public RampageSynergyEffect(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 0;
    }

    public override bool ProcessInteraction()
    {
        if ((double)caster.GetCurrentHitPoints() / (double)caster.GetMaximumHitPoints() <= rampageSynergyHealthThreshold)
        {
            ApplyEffect();
            return false;
        }
        else
        {
            return true;
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

    private void ApplyEffect()
    {
        if (caster.IsDead())
        {
            return;
        }

        caster.SetAttackSpeed(caster.GetAttackSpeed() + rampageDefaultAttackSpeedAmount);
        caster.SetArmourPercentage(caster.GetArmourPercentage() + rampageDefaultArmourPercentage);

        Interaction skill = new RampageSynergyLingeringEffect(caster);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has Rampaged-ed " + caster.GetName() + " to increase attack speed to " + caster.GetAttackSpeed() + " and armour to " + caster.GetArmourPercentage() + ".");
    }
}

public class RampageSynergyLingeringEffect : Interaction
{
    public Piece caster;
    private Vector3 attackDestination;

    public RampageSynergyLingeringEffect(Piece caster)
    {
        this.caster = caster;
        this.ticksRemaining = 999;
        interactionPrefab = Enums.InteractionPrefab.EviscerateBleed;
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

        if (!caster.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            attackDestination.y += 3.5f;
        }

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0 || caster.IsDead())
        {
            return false;
        }
        return true;
    }
}
