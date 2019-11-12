using UnityEngine;
using System;

public class MeleeAttack : Interaction
{
    private Piece attacker;
    private Piece target;
    private int damageToInflict;

    public MeleeAttack(Piece attacker, Piece target, int damageToInflict)
    {
        this.attacker = attacker;
        this.target = target;
        this.damageToInflict = damageToInflict;
    }

    public override bool ProcessInteraction()
    {
        ApplyDamageToInflict();
        return false;
    }

    public override void CleanUpInteraction() { }

    public override bool ProcessInteractionView()
    {
        return false;
    }

    private void ApplyDamageToInflict()
    {
        if (target.IsDead())
        {
            return;
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

        if (attacker.GetLifestealPercentage() > 0 && !attacker.invulnerable) // Undead synergy
        {
            attacker.SetCurrentHitPoints(Math.Min(attacker.GetMaximumHitPoints(),
                (int)Math.Floor((attacker.GetCurrentHitPoints() + damageToInflict * attacker.GetLifestealPercentage()))));
        }

        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - calculatedDamageToInflict);
        target.SetCurrentManaPoints(target.GetCurrentManaPoints() + target.GetManaPointsGainedOnDamaged());
        Debug.Log(attacker.GetName() + " has attacked " + target.GetName() + " for " + calculatedDamageToInflict + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
