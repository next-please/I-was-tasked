using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GuidingSpiritSynergyEffect : Interaction
{
    private Piece caster;
    private Board board;
    private Tile lastKnownTile;
    public int attackSpeed;
    public int attackDamage;
    private int lastKnownMana;
    public int ticksTilActivation = 0;

    public GuidingSpiritSynergyEffect(Piece caster, Board board, int attackDamage, int attackSpeed)
    {
        this.identifier = "GuidingSpirit";
        this.caster = caster;
        this.lastKnownTile = caster.GetCurrentTile();
        this.board = board;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.lastKnownMana = caster.GetCurrentManaPoints();
        this.ticksRemaining = ticksTilActivation;
        this.ticksTotal = 50;
    }

    public override bool ProcessInteraction()
    {
        if (!caster.IsDead())
        {
            lastKnownTile = caster.GetCurrentTile();
            lastKnownMana = caster.GetCurrentManaPoints();
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
        //interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    public void ApplyEffect()
    {
        Piece target = board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)];

        if (!target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() + lastKnownMana);
        }
        target.SetAttackSpeed(target.GetAttackSpeed() + attackSpeed);
        target.SetAttackDamage(target.GetAttackDamage() + attackDamage);
     
        board.AddInteractionToProcess(new GuidingSpiritLingeringEffect(lastKnownTile, target));
        if (target.interactions.Find(x => x.identifier.Equals("GuidingSpirit")) != null)
        {
            GuidingSpiritSynergyEffect skill = (GuidingSpiritSynergyEffect)target.interactions.Find(x => x.identifier.Equals("GuidingSpirit"));
            skill.attackDamage += attackDamage;
            skill.attackSpeed += attackSpeed;
        }

        Debug.Log("Guiding spirits has been sent with an attack speed of " + attackSpeed + " and an attack damage of " + attackDamage + ". It also healed for " + lastKnownMana + ".");
    }
}
public class GuidingSpiritLingeringEffect : Interaction
{
    private Vector3 startDestination;
    private Vector3 attackDestination;
    private Piece target;
    public static int ticksTilActivation = GameLogicManager.Inst.Data.Synergy.GuidingSpiritLingerTicks;

    public GuidingSpiritLingeringEffect(Tile startingTile, Piece targetPiece)
    {
        this.startDestination = ViewManager.CalculateTileWorldPosition(startingTile);
        startDestination.y = 1.0f;
        this.attackDestination = ViewManager.CalculateTileWorldPosition(targetPiece.GetCurrentTile());
        attackDestination.y = 1.0f;
        this.target = targetPiece;
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

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        if (!target.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 1.0f;
        }

        float fracJourney = (float)(ticksTilActivation - ticksRemaining) / ticksTilActivation;
        projectile.transform.position = Vector3.Lerp(startDestination, attackDestination, fracJourney);
        projectile.transform.LookAt(attackDestination);

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }
}
