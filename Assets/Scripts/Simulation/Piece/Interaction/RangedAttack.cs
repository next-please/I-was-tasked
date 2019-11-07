using UnityEngine;
using System;

public class RangedAttack : Interaction
{
    private Piece attacker;
    private Piece target;
    private Vector3 attackSource;
    private Vector3 attackDestination;
    private int damageToInflict;

    public RangedAttack(Piece attacker, Piece target, int damageToInflict, int ticksTotal)
    {
        this.attacker = attacker;
        this.target = target;
        this.damageToInflict = damageToInflict;
        this.ticksRemaining = ticksTotal;
        this.ticksTotal = ticksTotal;

        // Testing
        if (attacker.GetRace() == Enums.Race.Human && attacker.GetClass() == Enums.Job.Mage)
        {
            interactionPrefab = Enums.InteractionPrefab.ProjectileTestRed;
        }
        else
        {
            interactionPrefab = Enums.InteractionPrefab.ProjectileTestBlue;
        }

        attackSource = ViewManager.CalculateTileWorldPosition(attacker.GetCurrentTile());
        attackSource.y = 1.5f;
        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 1.5f;
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
        if (interactionView != null)
        {
            interactionView.CleanUpInteraction();
        }
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        if (!target.IsDead())
        {
            attackDestination = target.GetPieceView().transform.position;
            attackDestination.y = 1.5f;
        }

        float fracJourney = (float)(ticksTotal - ticksRemaining) / ticksTotal;
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

        if (attacker.GetLifestealPercentage() > 0 && !attacker.invulnerable) // Undead synergy
        {
            attacker.SetCurrentHitPoints(Math.Min(attacker.GetMaximumHitPoints(),
                (int) Math.Floor((attacker.GetCurrentHitPoints() + damageToInflict * attacker.GetLifestealPercentage()))));
        }

        if (attacker.GetCurseDamageAmount() > 0 && !attacker.invulnerable) //undead priest spell
        {
            attacker.SetCurrentHitPoints(attacker.GetCurrentHitPoints() - attacker.GetCurseDamageAmount());
        }

        if (target.GetLinkedProtectingPiece() != null) //elf knight spell
        {
            if (!target.GetLinkedProtectingPiece().IsDead())
            {
                target = target.GetLinkedProtectingPiece();
            }
        }

        int calculatedDamageToInflict = damageToInflict;

        if (target.GetBlockAmount() > 0) //orc druid spell
        {
            calculatedDamageToInflict -= Math.Max(damageToInflict, 0);
        }

        if (target.GetArmourPercentage() != 0) //undead mage spell && orc knight spell
        {
            calculatedDamageToInflict = (int)Math.Floor(damageToInflict / (1 + target.GetArmourPercentage()));
        }

        if (target.GetRecoilPercentage() > 0 && !attacker.invulnerable) // Knight synergy
        {
            attacker.SetCurrentHitPoints((int)Math.Ceiling(attacker.GetCurrentHitPoints() - calculatedDamageToInflict * target.GetRecoilPercentage()));
        }

        if (target.invulnerable)
        {
            calculatedDamageToInflict = 0;
        }

        target.SetCurrentManaPoints(target.GetCurrentManaPoints() + target.GetManaPointsGainedOnDamaged());
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - calculatedDamageToInflict);
        Debug.Log(attacker.GetName() + " has ranged attacked " + target.GetName() + " for " + calculatedDamageToInflict + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
