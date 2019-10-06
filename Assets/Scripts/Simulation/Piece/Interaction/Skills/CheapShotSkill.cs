using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapShotSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 50;
    public int cheapShotDefaultStunDuration = 250;

    public CheapShotSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestYellow;

        attackSource = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackSource.y = 2.0f;
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
            ApplyDamageToInflict();
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

        projectile.transform.position = attackSource;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (target.IsDead())
        {
            return;
        }

        if (target.GetState().ticksRemaining < cheapShotDefaultStunDuration)
        {
            target.GetState().ticksRemaining = cheapShotDefaultStunDuration;
        }

        Interaction skill = new CheapShotLingeringEffect(attackSource);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has CheapShot-ed " + target.GetName() + ", stunning them for " + cheapShotDefaultStunDuration + " ticks.");
    }
}

public class CheapShotLingeringEffect : Interaction
{
    private Vector3 effectPosition;
    public int ticksTilActivation = 250;

    public CheapShotLingeringEffect(Vector3 effectPosition)
    {
        this.effectPosition = effectPosition;
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

        projectile.transform.position = effectPosition;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }
}
