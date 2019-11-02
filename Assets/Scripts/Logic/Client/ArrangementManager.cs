﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class ArrangementManager : MonoBehaviour
{
    public BoardManager boardManager;
    public MarketManager marketManager;
    public InventoryManager inventoryManager;
    public RequestHandler requestHandler;
    public PhaseManager phaseManager;

    #region Bench To Board
    public bool CanMoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        Tile actualTile = boardManager.GetActualTile(player, tile);
        Piece actualPiece = inventoryManager.GetActualBenchPiece(player, piece);
        return phaseManager.IsMovablePhase() 
            && inventoryManager.BenchContainsPiece(player, actualPiece) 
            && !actualTile.IsOccupied();
    }

    public void TryMoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        if (!CanMoveBenchToBoard(player, piece, tile))
            return;
        Data data = new PieceMovementData(player, piece, tile);
        Request req = new Request(0, data); // TODO: replace with proper codes
        requestHandler.SendRequest(req);
    }

    public void MoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        if (!CanMoveBenchToBoard(player, piece, tile))
            return;
        Piece actualPiece = inventoryManager.GetActualBenchPiece(player, piece);
        inventoryManager.RemoveFromBench(player, actualPiece);
        inventoryManager.AddToArmy(player, actualPiece);
        boardManager.AddPieceToBoard(player, actualPiece, tile.GetRow(), tile.GetCol());
    }
    #endregion

    #region Board To Bench
    public bool CanMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        return phaseManager.IsMovablePhase() 
            && !inventoryManager.IsBenchFull(player) 
            && inventoryManager.IsBenchSlotVacant(player, slotIndex);
    }

    public void TryMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (!CanMoveBoardToBench(player, piece, slotIndex))
            return;
        BoardToBenchData data = new BoardToBenchData(player, piece, slotIndex);
        Request request = new Request(1, data);
        requestHandler.SendRequest(request);
    }

    public void MoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (!CanMoveBoardToBench(player, piece, slotIndex))
            return;
        // must be master client
        Piece actualPiece = boardManager.GetActualPiece(player, piece);
        boardManager.RemovePieceFromBoard(player, actualPiece);
        inventoryManager.RemoveFromArmy(player, actualPiece);
        inventoryManager.AddToBench(player, actualPiece);
        inventoryManager.MoveBenchPieceToIndex(player, actualPiece, slotIndex);
    }
    #endregion

    #region Move on Board
    public bool CanMovePieceOnBoard(Player player, Piece piece, Tile nextTile)
    {
        Tile actualNextTile = boardManager.GetActualTile(player, nextTile);
        Piece actualPiece = boardManager.GetActualPiece(player, piece);
        return phaseManager.IsMovablePhase() 
            && inventoryManager.ArmyContainsPiece(player, actualPiece) 
            && !actualNextTile.IsOccupied();
    }

    public void TryMovePieceOnBoard(Player player, Piece piece, Tile nextTile)
    {
        if (!CanMovePieceOnBoard(player, piece, nextTile))
            return;
        Data data = new PieceMovementData(player, piece, nextTile);
        Request req = new Request(2, data);
        requestHandler.SendRequest(req);
    }

    public void MovePieceOnBoard(Player player, Piece piece, Tile nextTile)
    {
        if (!CanMovePieceOnBoard(player, piece, nextTile))
            return;
        Piece actualPiece = inventoryManager.GetActualArmyPiece(player, piece);
        Tile actualNextTile = boardManager.GetActualTile(player, nextTile);
        boardManager.SetPieceAtTile(player, actualPiece, actualNextTile);
    }
    #endregion

    public void TryMovePieceOnBench(Player player, Piece piece, int index)
    {
        // must be master client
        Piece actualPiece = inventoryManager.GetActualBenchPiece(player, piece);
        inventoryManager.MoveBenchPieceToIndex(player, actualPiece, index);
    }
}
