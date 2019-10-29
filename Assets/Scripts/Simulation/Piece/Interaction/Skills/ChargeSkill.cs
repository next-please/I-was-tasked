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
    private Vector3 targetTilePos;
    private float speedToTranslate;
    public Vector3 forward;

    public ChargeSkill(Piece caster, Piece target, Board board)
    {
        this.caster = caster;
        this.target = target;
        this.board = board;

        int targetRow = target.GetCurrentTile().GetRow();
        int targetCol = target.GetCurrentTile().GetCol();
        int selfRow = caster.GetCurrentTile().GetRow();
        int selfCol = caster.GetCurrentTile().GetCol();

        interactionPrefab = Enums.InteractionPrefab.Charge;

        Tile tile = null;

        if (targetCol > 0 && selfCol < targetCol)
        {
            tile = board.GetTile(targetRow, targetCol - 1);
            if (!tile.IsLocked() && !tile.IsOccupied())
            {
                targetTile = tile;
            }
        }

        if (targetTile == null && targetCol + 1 < board.GetNumCols() && selfCol > targetCol)
        {
            tile = board.GetTile(targetRow, targetCol + 1);
            if (!tile.IsLocked() && !tile.IsOccupied())
            {
                targetTile = tile;
            }
        }

        if (targetTile == null && targetRow > 0 && selfRow < targetRow)
        {
            tile = board.GetTile(targetRow - 1, targetCol);
            if (!tile.IsLocked() && !tile.IsOccupied())
            {
                targetTile = tile;
            }
        }


        if (targetTile == null && targetRow + 1 < board.GetNumRows() && selfRow < targetRow)
        {
            tile = board.GetTile(targetRow + 1, targetCol);
            if (!tile.IsLocked() && !tile.IsOccupied())
            {
                targetTile = tile;
            }
        }

        if (targetTile == null)
        {
            this.ticksTotal = 0;
            this.ticksRemaining = 0;
        }
        else
        {
            Debug.Log(caster.GetName() + " chose this tile " + targetTile);
            int distance = caster.GetCurrentTile().DistanceToTile(targetTile);
            this.ticksTotal = 10 * distance;
            Vector3 currentTilePos = ViewManager.CalculateTileWorldPosition(caster.GetCurrentTile());
            targetTilePos = ViewManager.CalculateTileWorldPosition(targetTile);
            float distanceToTile = Vector3.Distance(currentTilePos, targetTilePos);
            float timeToReachTile = this.ticksTotal * FixedClock.Instance.deltaTime;
            speedToTranslate = distanceToTile / timeToReachTile;

            PieceView pieceView = caster.GetPieceView();
            pieceView.LookAtTile(targetTile);
            forward = Vector3.Normalize(targetTilePos - currentTilePos);
            board.MovePieceToTile(caster, targetTile); // move to tile straight away
            totalDamage = (int)Math.Floor(chargeBaseDefaultDamage * (Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
            this.ticksRemaining = this.ticksTotal;
            caster.GetPieceView().animator.Play("Walk");
        }
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
        PieceView pv = caster.GetPieceView();
        targetTilePos.y = 0.5f;
        pv.transform.position = targetTilePos;
        if (!target.IsDead())
        {
            pv.LookAtTile(target.GetCurrentTile());
        }
        interactionView.CleanUpInteraction();
    }

    public override bool ProcessInteractionView()
    {
        if (ticksRemaining > 0)
        {
            interactionView.transform.forward = forward;
            PieceView pv = caster.GetPieceView();
            pv.transform.Translate(forward * speedToTranslate * Time.deltaTime, Space.World);
            interactionView.transform.position = pv.transform.position;
            return true;
        }
        return false;
    }

    private void ApplyDamageToInflict()
    {
        if (caster.IsDead() || target.IsDead())
            return;

        target.SetCurrentHitPoints(target.GetCurrentHitPoints() - totalDamage);
        Debug.Log(caster.GetName() + " has Charge-ed " + target.GetName() + " for " + totalDamage + " DMG, whose HP has fallen to " + target.GetCurrentHitPoints() + " HP.");
    }
}
