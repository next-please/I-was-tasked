using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotSkill : Interaction
{
    private Piece caster;
    private Board board;
    private Vector3 attackSource;
    private int ticksTilActivation = 50;
    private int countRemaining;
    public int rotDefaultRadius = 1;
    public int rotDefaultCount = 20;
    public int rotDefaultDamage = 7;

    public RotSkill(Piece caster, Board board)
    {
        this.caster = caster;
        this.board = board;
        this.countRemaining = rotDefaultCount;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Rot;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;
    }

    public RotSkill(Piece caster, Board board, int countRemaining)
    {
        this.caster = caster;
        this.board = board;
        this.countRemaining = countRemaining;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.Rot;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;
    }

    public override bool ProcessInteraction()
    {
        ticksRemaining--;
        if (ticksRemaining == 0 && countRemaining > 0)
        {
            countRemaining--;
            ApplyDamageToInflict();
            if (caster.GetState().GetType() != typeof(InfiniteState) &&
                board.GetActiveEnemiesOnBoard().Count > 0)
            {
                ticksRemaining = ticksTilActivation;
            }
        }
        return ticksRemaining >= 0;
    }

    public override void CleanUpInteraction()
    {
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        GameObject projectile = interactionView.gameObject;

        // Projectile chases the Target. If the Target is dead, the Projectile will go to the Tile the Target was previously on.
        projectile.transform.position = attackSource;

        if (ticksRemaining <= 0 || caster.IsDead())
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (caster.IsDead())
        {
            return;
        }

        if (!caster.IsEnemy())
        {
            foreach (Piece target in board.GetActiveEnemiesWithinRadiusOfTile(caster.GetCurrentTile(), rotDefaultRadius))
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() - rotDefaultDamage);
            }
        }
        else
        {
            foreach (Piece target in board.GetActiveFriendliesWithinRadiusOfTile(caster.GetCurrentTile(), rotDefaultRadius))
            {
                target.SetCurrentHitPoints(target.GetCurrentHitPoints() - rotDefaultDamage);
            }
        }

        Debug.Log(caster.GetName() + " has Rot-ed targets around it for " + rotDefaultDamage + " DMG.");
    }
}
