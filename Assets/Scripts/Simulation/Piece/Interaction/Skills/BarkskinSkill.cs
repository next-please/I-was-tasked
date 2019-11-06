using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarkskinSkill : Interaction
{
    private Piece caster;
    private Board board;
    public static double barkskinDefaultReflectAmount = 0.25;
    public int ticksTilActivation = 0;

    public BarkskinSkill(Piece caster, Board board)
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

        if (caster.interactions.Find(x => x.identifier.Equals("Barkskin")) != null)
        {
            BarkskinLingeringEffect skill = (BarkskinLingeringEffect)caster.interactions.Find(x => x.identifier.Equals("Barkskin"));
            skill.ticksRemaining = BarkskinLingeringEffect.ticksTilActivation;
        }
        else
        {
            double reflectAmount = barkskinDefaultReflectAmount;
            caster.SetRecoilPercentage(caster.GetRecoilPercentage() + reflectAmount);

            Interaction skill = new BarkskinLingeringEffect(caster, reflectAmount);
            board.AddInteractionToProcess(skill);
            caster.interactions.Add(skill);
        }

        Debug.Log(caster.GetName() + " has Barkskin-ed " + caster.GetName() + " to increase recoil to " + caster.GetRecoilPercentage() + ".");
    }
}

public class BarkskinLingeringEffect : Interaction
{
    public Piece caster;
    private Vector3 attackDestination;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Skills.BarkSkinLingerTicks;
    public double reflectAmount;

    public BarkskinLingeringEffect(Piece caster, double reflectAmount)
    {
        this.identifier = "Barkskin";
        this.caster = caster;
        this.reflectAmount = reflectAmount;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.BarkSkin;
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
        GameObject projectile = interactionView.gameObject;

        if (!caster.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            attackDestination.y += 3.5f;
        }

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
        caster.SetRecoilPercentage(caster.GetRecoilPercentage() - reflectAmount);
        caster.interactions.Remove(caster.interactions.Find(x => x.identifier.Equals("Barkskin")));

        Debug.Log(caster.GetName() + "'s Barkskin has expired, recoil decreases to " + caster.GetRecoilPercentage() + ".");
    }

}
