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
    public double chargeStackingDefaultPercentageIncrease = GameLogicManager.Inst.Data.Skills.ChargeStackingPercentIncrease;
    public double chargeBaseDefaultDamage = GameLogicManager.Inst.Data.Skills.ChargeBaseDamage;
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
                else if (caster.GetCurrentTile().DistanceToTile(tilePotential) < caster.GetCurrentTile().DistanceToTile(targetTile))
                {
                    targetTile = tilePotential;
                }
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
            int damage = (int)Math.Floor(chargeBaseDefaultDamage * Math.Pow(GameLogicManager.Inst.Data.Skills.ChargeRarityMultiplier, caster.GetRarity()));
            totalDamage = (int)Math.Floor(damage * (Math.Pow(chargeStackingDefaultPercentageIncrease, distance)));
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
        if (!target.invulnerable)
        {
            caster.SetTarget(target);
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - totalDamage);
        }
        Debug.Log(caster.GetName() + " has Charge-ed " + target.GetName() + " for " + totalDamage + " DMG, whose HP has fallen to " + target.GetCurrentHitPoints() + " HP.");
    }
}
