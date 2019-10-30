using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShadowStrikeSkill : Interaction
{
    public Piece caster;
    public Piece target;
    private Board board;
    public Vector3 attackSource;
    public Vector3 attackDestination;
    public Tile targetTile;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.ShadowStrikeInitialTick;
    public int shadowStrikeDefaultDamage = GameLogicManager.Inst.Data.Skills.ShadowStrikeDamage;

    public ShadowStrikeSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ShadowStrike;

        int targetRow = target.GetCurrentTile().GetRow();
        int targetCol = target.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        if (!board.GetTile(targetRow + 1, targetCol).IsLocked() && !board.GetTile(targetRow + 1, targetCol).IsOccupied() && selfRow < targetRow)
        {
            targetTile = board.GetTile(targetRow + 1, targetCol);
        }
        else if (!board.GetTile(targetRow - 1, targetCol).IsLocked() && !board.GetTile(targetRow - 1, targetCol).IsOccupied() && selfRow > targetRow)
        {
            targetTile = board.GetTile(targetRow - 1, targetCol);
        }
        else if (!board.GetTile(targetRow, targetCol - 1).IsLocked() && !board.GetTile(targetRow, targetCol - 1).IsOccupied() && selfCol > targetCol)
        {
            targetTile = board.GetTile(targetRow, targetCol - 1);
        }
        else if (!board.GetTile(targetRow, targetCol + 1).IsLocked() && !board.GetTile(targetRow, targetCol + 1).IsOccupied() && selfCol < targetCol)
        {
            targetTile = board.GetTile(targetRow, targetCol + 1);
        }
        else
        {
            targetTile = caster.GetCurrentTile();
        }

        attackSource = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
        attackSource.y = 0.5f;

        attackDestination = ViewManager.CalculateTileWorldPosition(target.GetCurrentTile());
        attackDestination.y = 0.5f;

        Interaction skill = new ShadowStrikeLingeringEffect(attackDestination);
        board.AddInteractionToProcess(skill);
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

        projectile.transform.position = attackSource;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (caster.IsDead())
            return;

        int damage = (int)Math.Floor(shadowStrikeDefaultDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.ShadowStrikeRarityMultiplier, caster.GetRarity()));
        if (!target.IsDead() && !target.invulnerable)
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - damage);
            Debug.Log(caster.GetName() + " has ShadowStrike-ed " + target.GetName() + " for " + damage + " DMG, whose HP has fallen to " + target.GetCurrentHitPoints() + " HP.");
        }

        board.MovePieceToTile(caster, targetTile);
    }
}

public class ShadowStrikeLingeringEffect : Interaction
{
    private Vector3 effectPosition;
    public int ticksTilActivation = 40;

    public ShadowStrikeLingeringEffect(Vector3 effectPosition)
    {
        this.effectPosition = effectPosition;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ProjectileTestBlack;
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

        projectile.transform.position = effectPosition;

        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }
}
