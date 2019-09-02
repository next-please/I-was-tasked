using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangementManager : MonoBehaviour
{
    public BoardManager boardManager;
    public InventoryManager inventoryManager;

    public void TryMovePieceOnBoard(Player player, Piece piece, Tile nextTile)
    {
        boardManager.MovePieceToTile(player, piece, nextTile);
    }

    public void TryMoveBenchToBoard(Player player, Piece piece, Tile tile)
    {
        if (!inventoryManager.BenchContainsPiece(player, piece) ||
             tile.IsOccupied())
        {
            return;
        }

        // must be master client
        inventoryManager.RemoveFromBench(player, piece);
        boardManager.AddPieceToBoard(player, piece, tile.GetRow(), tile.GetCol());
    }

    public void TryMoveBoardToBench(Player player, Piece piece, int slotIndex)
    {
        if (inventoryManager.IsBenchFull(player))
        {
            return;
        }
        // must be master client
        boardManager.RemovePieceFromBoard(player, piece);
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
    }
}
