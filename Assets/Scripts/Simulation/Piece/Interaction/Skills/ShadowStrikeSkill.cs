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
    public Tile targetTile = null;
    private int ticksTilActivation = GameLogicManager.Inst.Data.Skills.ShadowStrikeInitialTick;
    public static int shadowStrikeDefaultDamage = GameLogicManager.Inst.Data.Skills.ShadowStrikeDamage;
    public int damage;

    public ShadowStrikeSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.damage = shadowStrikeDefaultDamage;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ShadowStrike;

        int targetRow = target.GetCurrentTile().GetRow();
        int targetCol = target.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        List<Tile> tiles = new List<Tile>();
        if (targetCol + 1 < board.GetNumCols())
            tiles.Add(board.GetTile(targetRow, targetCol + 1));
        if (targetCol - 1 >= 0)
            tiles.Add(board.GetTile(targetRow, targetCol - 1));
        if (targetRow + 1 < board.GetNumRows())
            tiles.Add(board.GetTile(targetRow + 1, targetCol));
        if (targetRow - 1 >= 0)
            tiles.Add(board.GetTile(targetRow - 1, targetCol));

        foreach (Tile tilePotential in tiles)
        {
            if (!tilePotential.IsLocked() && !tilePotential.IsOccupied())
            {
                if (targetTile == null)
                {
                    targetTile = tilePotential;
                }
                else if (caster.GetCurrentTile().DistanceToTile(tilePotential) > caster.GetCurrentTile().DistanceToTile(targetTile))
                {
                    targetTile = tilePotential;
                }
            }
        }

        if (targetTile == null)
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

    public ShadowStrikeSkill(Piece caster, Piece target, Board board, int damage)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.damage = damage;
        this.ticksRemaining = ticksTilActivation;
        interactionPrefab = Enums.InteractionPrefab.ShadowStrike;

        int targetRow = target.GetCurrentTile().GetRow();
        int targetCol = target.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        List<Tile> tiles = new List<Tile>();
        if (targetCol + 1 < board.GetNumCols())
            tiles.Add(board.GetTile(targetRow, targetCol + 1));
        if (targetCol - 1 >= 0)
            tiles.Add(board.GetTile(targetRow, targetCol - 1));
        if (targetRow + 1 < board.GetNumRows())
            tiles.Add(board.GetTile(targetRow + 1, targetCol));
        if (targetRow - 1 >= 0)
            tiles.Add(board.GetTile(targetRow - 1, targetCol));

        foreach (Tile tilePotential in tiles)
        {
            if (!tilePotential.IsLocked() && !tilePotential.IsOccupied())
            {
                if (targetTile == null)
                {
                    targetTile = tilePotential;
                }
                else if (caster.GetCurrentTile().DistanceToTile(tilePotential) > caster.GetCurrentTile().DistanceToTile(targetTile))
                {
                    targetTile = tilePotential;
                }
            }
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

        int damage = (int)Math.Floor(this.damage * Math.Pow(GameLogicManager.Inst.Data.Skills.ShadowStrikeRarityMultiplier, caster.GetRarity()));
        if (!target.IsDead() && !target.invulnerable)
        {
            caster.SetTarget(target);
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
