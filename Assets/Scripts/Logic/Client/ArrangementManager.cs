using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class ArrangementManager : MonoBehaviour
{
    public BoardManager boardManager;
    public MarketManager marketManager;
    public InventoryManager inventoryManager;
    public RequestHandler requestHandler;

    public void TryMovePieceOnBoard(Player player, Piece piece, Tile nextTile)
    {
        boardManager.MovePieceToTile(player, piece, nextTile);
    }

#region Bench To Board
    public bool CanMoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        Tile actualTile = boardManager.GetActualTile(player, tile);
        return inventoryManager.BenchContainsPiece(player, piece) &&
               !actualTile.IsOccupied();
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
        inventoryManager.RemoveFromBench(player, piece);
        inventoryManager.AddToArmy(player, piece);
        boardManager.AddPieceToBoard(player, piece, tile.GetRow(), tile.GetCol());
    }
#endregion

#region Board To Bench
    public bool CanMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        return !inventoryManager.IsBenchFull(player) &&
               inventoryManager.IsBenchSlotVacant(player, slotIndex);
    }

    public void TryMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (CanMoveBoardToBench(player, piece, slotIndex))
          return;
        BoardToBenchData data = new BoardToBenchData(player, piece, slotIndex);
        Request request = new Request(1, data);
        requestHandler.SendRequest(request);
    }

    public void MoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (CanMoveBoardToBench(player, piece, slotIndex))
            return;
        // must be master client
        boardManager.RemovePieceFromBoard(player, piece);
        inventoryManager.RemoveFromArmy(player, piece);
        inventoryManager.AddToBench(player, piece);
        inventoryManager.MoveBenchPieceToIndex(player, piece, slotIndex);
    }
#endregion

    public void TryMovePieceOnBench(Player player, Piece piece, int index)
    {
        // must be master client
        inventoryManager.MoveBenchPieceToIndex(player, piece, index);
    }

    public void TryRemovePieceOnBoard(Player player, Piece piece)
    {
        boardManager.RemovePieceFromBoard(player, piece);
        marketManager.characterGenerator.ReturnPiece(piece);
    }



}
