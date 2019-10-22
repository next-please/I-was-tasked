using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostArmourSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public double frostArmourDefaultArmourPercentage = 0.3;
    public int ticksTilActivation = 0;

    public FrostArmourSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
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
        target.SetArmourPercentage(target.GetArmourPercentage() + frostArmourDefaultArmourPercentage);
        double armourChange = frostArmourDefaultArmourPercentage;

        Interaction skill = new FrostArmourLingeringEffect(caster, armourChange);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has FrostArmour-ed " + target.GetName() + " to increase armour to " + target.GetArmourPercentage() + ".");
    }
}

public class FrostArmourLingeringEffect : Interaction
{
    private Piece target;
    private double armourChange;
    private Vector3 attackDestination;
    public int ticksTilActivation = 250;
    public int blockAmount;

    public FrostArmourLingeringEffect(Piece target, double armourChange)
    {
        this.target = target;
        this.armourChange = armourChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.FrostArmour;
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
        if (!target.IsDead())
        {
            GameObject projectile = interactionView.gameObject;
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y += 1.0f;
            projectile.transform.position = attackDestination;
        }

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        if (target.IsDead())
        {
            return;
        }
        target.SetArmourPercentage(target.GetArmourPercentage() - armourChange);

        Debug.Log(target.GetName() + "'s Frost Armour has expired.");
    }

}