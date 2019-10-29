using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlessingOfNatureSkill : Interaction
{
    private Piece caster;
    private Board board;
    public double blessingOfNatureDefaultMultiplierIncrease = GameLogicManager.Inst.Data.Skills.BlessingOfNatureMultiplierIncrease;
    public int ticksTilActivation = 0;

    public BlessingOfNatureSkill(Piece caster, Board board)
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

        int targetIndex;
        if (!caster.IsEnemy())
        {
           targetIndex = board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count - 1);
        }
        else
        {
            targetIndex = board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count - 1);
        }

        Piece target;
        if (!caster.IsEnemy())
        {
            if (board.GetActiveFriendliesOnBoard()[targetIndex].Equals(caster))
            {
                target = board.GetActiveFriendliesOnBoard()[board.GetActiveFriendliesOnBoard().Count - 1];
            }
            else
            {
                target = board.GetActiveFriendliesOnBoard()[targetIndex];
            }
        }
        else
        {
            if (board.GetActiveEnemiesOnBoard()[targetIndex].Equals(caster))
            {
                target = board.GetActiveEnemiesOnBoard()[board.GetActiveEnemiesOnBoard().Count - 1];
            }
            else
            {
                target = board.GetActiveEnemiesOnBoard()[targetIndex];
            }
        }

        int attackDamageChange = (int)Math.Floor(target.GetAttackDamage() * blessingOfNatureDefaultMultiplierIncrease);
        target.SetAttackDamage(target.GetAttackDamage() + attackDamageChange);
        int currentHitPointChange = (int)Math.Floor(target.GetCurrentHitPoints() * blessingOfNatureDefaultMultiplierIncrease);
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() + currentHitPointChange);
        int maximumHitPointChange = (int)Math.Floor(target.GetMaximumHitPoints() * blessingOfNatureDefaultMultiplierIncrease);
        target.SetMaximumHitPoints(target.GetMaximumHitPoints() + maximumHitPointChange);

        Interaction skill = new BlessingOfNatureLingeringEffect(target, attackDamageChange, currentHitPointChange, maximumHitPointChange);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has BlessingOfNatured-ed " + target.GetName() + " to increase attack to " + target.GetAttackDamage() + " and hitpoints to " + target.GetMaximumHitPoints() + ".");
    }
}

public class BlessingOfNatureLingeringEffect : Interaction
{
    private Piece target;
    private int attackDamageChange;
    private int currentHitPointChange;
    private int maximumHitPointChange;
    private Vector3 attackDestination;
    public int ticksTilActivation = GameLogicManager.Inst.Data.Skills.BlessingOfNatureLingerTicks;
    public int blockAmount;

    public BlessingOfNatureLingeringEffect(Piece target, int attackDamageChange, int currentHitPointChange, int maximumHitPointChange)
    {
        this.target = target;
        this.attackDamageChange = attackDamageChange;
        this.currentHitPointChange = currentHitPointChange;
        this.maximumHitPointChange = maximumHitPointChange;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.BlessingOfNature;
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
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y += 1.0f;
        }

        projectile.transform.position = attackDestination;

        if (ticksRemaining <= 0 || target.IsDead())
        {
            return false;
        }
        return true;
    }

    private void ApplyEffect()
    {
        target.SetMaximumHitPoints(target.GetMaximumHitPoints() - maximumHitPointChange);
        target.SetAttackDamage(target.GetAttackDamage() - attackDamageChange);
        if (target.IsDead())
        {
            return;
        }
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - currentHitPointChange);

        Debug.Log(target.GetName() + "'s Blessing of Nature has expired.");
    }

}
