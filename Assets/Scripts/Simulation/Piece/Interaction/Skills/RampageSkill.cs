using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RampageSkill : Interaction
{
    private Piece caster;
    private Board board;
    public int rampageDefaultAttackSpeedAmount = GameLogicManager.Inst.Data.Skills.RampageAttackSpeed;
    public double rampageDefaultArmourPercentage = GameLogicManager.Inst.Data.Skills.RampageArmourPercentage;
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

        if (caster.interactions.Find(x => x.identifier.Equals("Rampage")) != null)
        {
            caster.interactions.Find(x => x.identifier.Equals("Rampage")).ticksRemaining = RampageLingeringEffect.ticksTilActivation;
        }
        else
        {
            int attackSpeedChange = caster.SetAttackSpeed(caster.GetAttackSpeed() + rampageDefaultAttackSpeedAmount);
            caster.SetArmourPercentage(caster.GetArmourPercentage() + rampageDefaultArmourPercentage);
            double armourChange = rampageDefaultArmourPercentage;

            Interaction skill = new RampageLingeringEffect(caster, attackSpeedChange, armourChange);
            board.AddInteractionToProcess(skill);
            caster.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has Rampaged-ed " + caster.GetName() + " to increase attack speed to " + caster.GetAttackSpeed() + " and armour to " + caster.GetArmourPercentage() + ".");
    }
}

public class RampageLingeringEffect : Interaction
{
    public Piece caster;
    private int attackSpeedChange;
    private double armourChange;
    private Vector3 attackDestination;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.RampageLingerTicks;
    public int blockAmount;

    public RampageLingeringEffect(Piece caster, int attackSpeedChange, double armourChange)
    {
        this.identifier = "Rampage";
        this.caster = caster;
        this.attackSpeedChange = attackSpeedChange;
        this.armourChange = armourChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Rampage;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0 && !caster.IsDead())
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
            interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        if (!caster.IsDead())
        {
            Transform casterT = caster.GetPieceView().transform;
            projectile.transform.parent =  casterT;
            Vector3 pos = Vector3.zero;
            pos.y = 3.5f;
            projectile.transform.localPosition = pos;
            projectile.transform.position = attackDestination;
        }

        if (ticksRemaining <= 0 || caster.IsDead())
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        caster.SetAttackSpeed(caster.GetAttackSpeed() - attackSpeedChange);
        caster.SetArmourPercentage(caster.GetArmourPercentage() - armourChange);
        caster.interactions.Remove(caster.interactions.Find(x => x.identifier.Equals("Rampage")));

        Debug.Log(caster.GetName() + "'s Rampage has expired.");
    }
}
