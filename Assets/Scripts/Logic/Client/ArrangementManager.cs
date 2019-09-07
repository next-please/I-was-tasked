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

    public void TryMoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        Data data = new PieceMovementData(player, piece, tile);
        Request req = new Request(player.ToString(), 0, data);
        requestHandler.SendRequest(req);
    }

    public void TryMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (inventoryManager.IsBenchFull(player))
        {
            return;
        }
        // must be master client
        boardManager.RemovePieceFromBoard(player, piece);
        inventoryManager.RemoveFromArmy(player, piece);
        inventoryManager.AddToBench(player, piece);
        inventoryManager.MoveBenchPieceToIndex(player, piece, slotIndex);
    }

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

    public void MoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
         // must be master client
        inventoryManager.RemoveFromBench(player, piece);
        inventoryManager.AddToArmy(player, piece);
        boardManager.AddPieceToBoard(player, piece, tile.GetRow(), tile.GetCol());

    }

    public bool IsValidBenchToBoard(Player player, Piece piece, Tile tile)
    {
        return !inventoryManager.BenchContainsPiece(player, piece) || tile.IsOccupied();
    }
}
