using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.VFX;

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

        if (target.interactions.Find(x => x.identifier.Equals("GuidingSpirit")) != null)
        {
            GuidingSpiritSynergyEffect skill = (GuidingSpiritSynergyEffect)target.interactions.Find(x => x.identifier.Equals("GuidingSpirit"));
            skill.attackDamage += attackDamage;
            skill.attackSpeed += attackSpeed;
        }

        List<Interaction> lingers = (List<Interaction>)caster.interactions.FindAll(x => x.identifier.Equals("GuidingSpiritLinger"));

        if (lingers.Count > 0)
        {
            foreach (var l in lingers)
            {
               GuidingSpiritLingeringEffect lingerEffect = l as GuidingSpiritLingeringEffect;
               lingerEffect.SetPath(lastKnownTile, target);
            }
        }
        GuidingSpiritLingeringEffect linger = new GuidingSpiritLingeringEffect(lastKnownTile, target);
        board.AddInteractionToProcess(linger);
        target.interactions.Add(linger);

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
        SetPath(startingTile, targetPiece);
        this.identifier = "GuidingSpiritLinger";
        interactionPrefab = Enums.InteractionPrefab.GuidingSpirits;
        this.ticksRemaining = ticksTilActivation;
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
        }
        // only cleared on board clear process
        return true;
    }

    public void SetPath(Tile startTile, Piece target)
    {
        this.startDestination = ViewManager.CalculateTileWorldPosition(startTile);
        startDestination.y = 3.0f;
        this.attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 3.0f;
        // this.ticksRemaining = ticksTilActivation;
        this.ticksRemaining = ticksTilActivation;
        this.target = target;
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on and be destroyed
        if (!target.IsDead())
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 3.0f;
        }

        float fracJourney = (float)(ticksTilActivation - ticksRemaining) / ticksTilActivation;
        projectile.transform.position = Vector3.Lerp(startDestination, attackDestination, fracJourney);
        projectile.transform.LookAt(attackDestination);
        return true;
    }
}
