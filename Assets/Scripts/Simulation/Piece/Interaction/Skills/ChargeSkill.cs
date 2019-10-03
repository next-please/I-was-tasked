using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChargeSkill : Interaction
{
    private Piece caster;
    private Piece target;
    private Board board;
    private Tile targetTile;
    private int ticksTilActivation = 6;
    public double chargeStackingDefaultPercentageIncrease = 1.2;
    public double chargeBaseDefaultDamage = 40;
    private int totalDamage;

    public ChargeSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;

        int targetRow = target.GetCurrentTile().GetRow();
        int targetCol = target.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        if (!board.GetTile(targetRow, targetCol - 1).IsLocked() && !board.GetTile(targetRow, targetCol - 1).IsOccupied() && selfCol < targetCol)
        {
            targetTile = board.GetTile(targetRow, targetCol - 1);
        }
        else if (!board.GetTile(targetRow, targetCol + 1).IsLocked() && !board.GetTile(targetRow, targetCol + 1).IsOccupied() && selfCol > targetCol)
        {
            targetTile = board.GetTile(targetRow, targetCol + 1);
        }
        else if (!board.GetTile(targetRow + 1, targetCol).IsLocked() && !board.GetTile(targetRow + 1, targetCol).IsOccupied() && selfRow > targetRow)
        {
            targetTile = board.GetTile(targetRow + 1, targetCol);
        }
        else if (!board.GetTile(targetRow - 1, targetCol).IsLocked() && !board.GetTile(targetRow - 1, targetCol).IsOccupied() && selfRow < targetRow)
        {
            targetTile = board.GetTile(targetRow - 1, targetCol);
        }
        else
        {
            targetTile = caster.GetCurrentTile();
        }
        int distance = caster.GetCurrentTile().DistanceToTile(targetTile);

        targetTile.SetLocker(caster);

        totalDamage = (int)Math.Floor(chargeBaseDefaultDamage * (Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
    }

    public ChargeSkill(Piece caster, Piece target, Board board, Tile targetTile, int totalDamage)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;
        this.ticksTotal = 50;
        this.ticksRemaining = ticksTilActivation;
        this.targetTile = targetTile;
        this.totalDamage = totalDamage;
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
        if (ticksRemaining <= 0)
        {
            return false;
        }
        return true;
    }

    private void ApplyDamageToInflict()
    {
        if (caster.IsDead() || target.IsDead())
            return;

        int newRow = caster.GetCurrentTile().GetRow();
        int newCol = caster.GetCurrentTile().GetCol();

        if (caster.GetCurrentTile().GetRow() < targetTile.GetRow())
        {
            newRow++;
        }
        if (caster.GetCurrentTile().GetRow() > targetTile.GetRow())
        {
            newRow--;
        }
        if (caster.GetCurrentTile().GetCol() < targetTile.GetCol())
        {
            newCol++;
        }
        if (caster.GetCurrentTile().GetCol() > targetTile.GetCol())
        {
            newCol--;
        }
        board.MovePieceToTile(caster, board.GetTile(newRow, newCol));

        if (caster.GetCurrentTile().GetCol() == targetTile.GetCol() && caster.GetCurrentTile().GetRow() == targetTile.GetRow())
        {
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - totalDamage);
            Debug.Log(caster.GetName() + " has Charge-ed " + target.GetName() + " for " + totalDamage + " DMG, whose HP has fallen to " + target.GetCurrentHitPoints() + " HP.");
        }
        else
        {
            Interaction skill = new ChargeSkill(caster, target, board, targetTile, totalDamage);
            board.AddInteractionToProcess(skill);
        }
    }
}
