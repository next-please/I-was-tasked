using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicMissileSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    public int magicMissileDefaultCount = GameLogicManager.Inst.Data.Skills.MagicMissileCount;
    public int magicMissileDefaultDamage = GameLogicManager.Inst.Data.Skills.MagicMissileDamage;
    private Vector3 attackSource;
    private Vector3 attackDestination;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.MagicMissileTicks;
    private int countRemaining;


    public MagicMissileSkill(Piece caster, Piece target, Board board, bool infinite = false)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.countRemaining = magicMissileDefaultCount;
        this.ticksTotal = 50;
        if (infinite)
        {
            this.ticksTotal = 999999999;
            this.countRemaining = 999999999;
        }
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.MagicMissile;

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
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.MagicMissile;

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
        int damage = (int)Math.Floor(magicMissileDefaultDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.MagicMissileRarityMultiplier, caster.GetRarity()));
        if (!target.IsDead() && !target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damage);
        }

        if (countRemaining > 0)
        {
            if (!caster.IsDead() && caster.GetState().GetType() != typeof(InfiniteState) && board.GetActiveEnemiesOnBoard().Count > 0 && !caster.IsEnemy())
            {
                Interaction skill = new MagicMissileSkill(caster, board.GetActiveEnemiesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveEnemiesOnBoard().Count)], board, countRemaining - 1);
                board.AddInteractionToProcess(skill);
            }
            else if (!caster.IsDead() && caster.GetState().GetType() != typeof(InfiniteState) && board.GetActiveEnemiesOnBoard().Count > 0 && caster.IsEnemy())
            {
                Interaction skill = new MagicMissileSkill(caster, board.GetActiveFriendliesOnBoard()[board.GetRNGesus().Next(0, board.GetActiveFriendliesOnBoard().Count)], board, countRemaining - 1);
                board.AddInteractionToProcess(skill);
            }

        }

        Debug.Log(caster.GetName() + " has MagicMissile-ed " + target.GetName() + " for " + damage + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
    }
}
