using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkForDeathSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    private Vector3 attackDestination;
    public int ticksTilActivation = 250;

    public MarkForDeathSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestBlack;
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

        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
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
        if (target.IsDead())
        {
            return;
        }
        target.SetCurrentHitPoints(0);

        Debug.Log(caster.GetName() + " has MarkForDeath-ed " + target.GetName() + ".");
    }
}
