using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterHealSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 20;
    public int greaterHealDefaultHeal = 150;

    public GreaterHealSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestYellow;

        List<Piece> damagedFriendlies = board.GetActiveFriendliesOnBoard().FindAll(piece => piece.GetCurrentHitPoints() < piece.GetMaximumHitPoints());
        if (damagedFriendlies.Count > 0)
        {
            this.target = damagedFriendlies[board.GetRNGesus().Next(0, damagedFriendlies.Count)];
        }
        else
        {
            this.target = board.GetActiveFriendliesOnBoard()[0];
        }

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

        target.SetCurrentHitPoints(target.GetCurrentHitPoints() + greaterHealDefaultHeal);

        Interaction skill = new GreaterHealLingeringEffect(attackSource);
        board.AddInteractionToProcess(skill);

        Debug.Log(caster.GetName() + " has GreaterHealed-ed " + target.GetName() + " for " + greaterHealDefaultHeal + " DMG, whose HP has risen to " + target.GetCurrentHitPoints() + " HP.");
    }
}

public class GreaterHealLingeringEffect : Interaction
{
    private Vector3 effectPosition;
    public int ticksTilActivation = 50;

    public GreaterHealLingeringEffect(Vector3 effectPosition)
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
