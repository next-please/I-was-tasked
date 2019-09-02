using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public MarketManager marketManager;
    public void TryToPurchaseMarketPieceToBench(Player player, Piece piece)
    {
        int price = piece.GetRarity();

        // check locally if we can do this transaction for immediate feedback
        if (inventoryManager.IsBenchFull(player) ||
            !inventoryManager.EnoughGoldToPurchase(player, price))
        {
            return;
        }

        // send over the network
        // if i'm not master:
        // INetworkManager.PurchaseMarketPieceToBench

        // if i'm master
        marketManager.RemoveMarketPiece(piece);
        inventoryManager.AddToBench(player, piece);
        inventoryManager.DeductGold(player, price);
    }

    public void TrySellBenchPiece(Player player, Piece piece)
    {
        // if i'm master
        inventoryManager.RemoveFromBench(player, piece);
    }

}
