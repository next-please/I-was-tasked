﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireblastSkill : Interaction
{
    private Piece caster;
    private Piece target;
    public int fireblastDefaultDamage = 150;
    private Vector3 attackSource;
    private Vector3 attackDestination;
    private int ticksTilActivation = 120;


    public FireblastSkill(Piece caster, Piece target)
    {
        this.caster = caster;
        this.target = target;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Fireblast;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 1.0f;
        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 1.0f;
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

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        if (target != null && !target.IsDead() && (target.GetCurrentTile().GetRow() != (int)attackDestination.x || target.GetCurrentTile().GetCol() != (int)attackDestination.z))
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 1.0f;
        }

        float fracJourney = (float)(ticksTilActivation - ticksRemaining) / ticksTilActivation;
        projectile.transform.position = Vector3.Lerp(attackSource, attackDestination, fracJourney);
        projectile.transform.LookAt(attackDestination);

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
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - fireblastDefaultDamage);

        Debug.Log(caster.GetName() + " has fireblasted-ed " + target.GetName() + " for " + fireblastDefaultDamage + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
