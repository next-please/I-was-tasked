using UnityEngine;
using UnityEditor;
using System;

public class Attack : Interaction
{
    public Piece attacker;
    public Piece target;
    public Vector3 attackSource;
    public Vector3 attackDestination;
    public int damageToInflict;
    public int ticksRemaining;
    public int ticksTotal;

    private GameObject projectile;

    public Attack(Piece attacker, Piece target, int damageToInflict, int ticksTotal)
    {
        this.attacker = attacker;
        this.target = target;
        this.damageToInflict = damageToInflict;
        this.ticksRemaining = ticksTotal;
        this.ticksTotal = ticksTotal;

        if (ticksTotal > 0)
        {
            projectile = MonoBehaviour.Instantiate((GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Pieces/ProjectileTest.prefab", typeof(GameObject)));
            attackSource = ViewManager.CalculateTileWorldPosition(attacker.GetCurrentTile());
            attackSource.y = 0.5f;
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 0.5f;
        }
    }

    public override bool ProcessInteraction()
    {
        if (ticksRemaining > 0)
        {
            ticksRemaining--;
            UpdateProjectileView();
            return true;
        }
        else
        {
            DestroyProjectileView();
            ApplyDamageToInflict();
            return false;
        }
    }

    public override void CleanUpInteraction()
    {
        DestroyProjectileView();
    }

    public void UpdateProjectileView()
    {
        if (projectile == null)
        {
            return;
        }

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        if (!target.IsDead() && (target.GetCurrentTile().GetRow() != (int) attackDestination.x || target.GetCurrentTile().GetCol() != (int) attackDestination.z))
        {
            attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
            attackDestination.y = 0.5f;
        }

        float fracJourney = (float) (ticksTotal - ticksRemaining) / ticksTotal;
        projectile.transform.position = Vector3.Lerp(attackSource, attackDestination, fracJourney);
        projectile.transform.LookAt(attackDestination);

        if (ticksRemaining <= 0)
        {
            DestroyProjectileView();
        }
    }

    public void DestroyProjectileView()
    {
        if (projectile != null)
        {
            MonoBehaviour.Destroy(projectile);
            projectile = null;
        }
    }

    public void ApplyDamageToInflict()
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
            attacker.SetCurrentHitPoints((int) Math.Ceiling(attacker.GetCurrentHitPoints() - damageToInflict * target.GetRecoilPercentage()));
        }

        target.SetCurrentManaPoints(target.GetCurrentManaPoints() + target.GetManaPointsGainedOnDamaged());
        Debug.Log(attacker.GetName() + " has attacked " + target.GetName() + " for " + damageToInflict + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
