using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int magicMissileDefaultCount = 3;
    public int magicMissileDefaultDamage = 40;
    private Vector3 attackSource;
    private Vector3 attackDestination;
    private int ticksTilActivation = 30;
    private int countRemaining;


    public MagicMissileSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.countRemaining = magicMissileDefaultCount;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestArcanePurple;

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 1.0f;
        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 1.0f;
    }

    public MagicMissileSkill(Piece caster, Piece target, Board board, int countRemaining)
    {
        this.caster = caster;
        this.target = target;
        this.countRemaining = countRemaining;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestArcanePurple;

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
        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - magicMissileDefaultDamage);

        if (countRemaining > 0)
        {
            if (!caster.IsDead() && caster.GetState().GetType() != typeof(InfiniteState))
            {
                Interaction skill = new MagicMissileSkill(caster, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board, countRemaining - 1);
                board.AddInteractionToProcess(skill);
            }
        }

        Debug.Log(caster.GetName() + " has MagicMissile-ed " + target.GetName() + " for " + magicMissileDefaultDamage + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
