using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProtectAllySkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public double shapeshiftDefaultMultiplierIncrease = 1.2;
    public int shapeshiftDefaultAttackSpeedIncrease = 1;
    public int ticksTilActivation = 0;

    public ProtectAllySkill(Piece caster, Board board)
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
        int targetIndex = 0;//rngesus.Next(0, board.GetActiveFriendliesOnBoard().Count - 1);
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
            target.SetLinkedProtectingPiece(ref caster);

            Interaction skill = new ProtectAllyLingeringEffect(target);
            board.AddInteractionToProcess(skill);

            Debug.Log(caster.GetName() + " has ProtectAlly-ed " + target.GetName() + " to take damage from attacks instead of them.");
        }
    }
}

public class ProtectAllyLingeringEffect : Interaction
{
    private Piece target;
    private Vector3 attackDestination;
    public int ticksTilActivation = 100;

    public ProtectAllyLingeringEffect(Piece target)
    {
        this.target = target;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestYellow;
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

        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
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
