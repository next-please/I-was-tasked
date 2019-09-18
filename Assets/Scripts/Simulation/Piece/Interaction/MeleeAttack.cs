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

        attacker.SetCurrentManaPoints(attacker.GetCurrentManaPoints() + attacker.GetManaPointsGainedOnAttack());
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damageToInflict);

        if (attacker.GetLifestealPercentage() > 0) // Undead synergy
        {
            attacker.SetCurrentHitPoints(Math.Min(attacker.GetMaximumHitPoints(),
                (int) Math.Floor((attacker.GetCurrentHitPoints() + damageToInflict * attacker.GetLifestealPercentage()))));
        }

        if (target.GetRecoilPercentage() > 0) // Knight synergy
        {
            attacker.SetCurrentHitPoints((int)Math.Ceiling(attacker.GetCurrentHitPoints() - damageToInflict * target.GetRecoilPercentage()));
        }

        target.SetCurrentManaPoints(target.GetCurrentManaPoints() + target.GetManaPointsGainedOnDamaged());
        Debug.Log(attacker.GetName() + " has melee attacked " + target.GetName() + " for " + damageToInflict + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
