using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public MarketManager marketManager;
    public IncomeManager incomeManager;

    public readonly int UpgradeIncomeCost = 10;
    public readonly int UpgradeMarketSizeCost = 10;

    public void TryToPurchaseMarketPieceToBench(Player player, Piece piece)
    {

        // check locally if we can do this transaction for immediate feedback
        if (inventoryManager.IsBenchFull(player) ||
            !inventoryManager.EnoughGoldToPurchase(player, piece.GetPrice()))
        {
            return;
        }

        // send over the network
        // if i'm not master:
        // INetworkManager.PurchaseMarketPieceToBench

        // if i'm master
        marketManager.RemoveMarketPiece(piece);
        inventoryManager.AddToBench(player, piece);
        inventoryManager.DeductGold(player, piece.GetPrice());
    }

    public void TrySellBenchPiece(Player player, Piece piece)
    {
        // if i'm master
        if (inventoryManager.RemoveFromBench(player, piece))
        {
            marketManager.characterGenerator.ReturnPiece(piece);
            inventoryManager.AddGold(player, piece.GetPrice());
        }
    }

    public void SellPiece(Player player, Piece piece)
    {
        marketManager.characterGenerator.ReturnPiece(piece);
        inventoryManager.AddGold(player, piece.GetPrice());
    }

    public void TryPurchaseIncreaseMarketRarity(Player player)
    {
        int price = GetMarketRarityCost();
        if (!inventoryManager.EnoughGoldToPurchase(player, price))
        {
            return;
        }
        marketManager.IncreaseMarketTier();
        inventoryManager.DeductGold(player, price);
    }

    public void TryPurchaseIncreasePassiveIncome(Player player)
    {
        int price = UpgradeIncomeCost;
        if (!inventoryManager.EnoughGoldToPurchase(player, price))
        {
            return;
        }
        incomeManager.IncreasePassiveIncome();
        inventoryManager.DeductGold(player, price);
    }

    public void TryPurchaseIncreaseMarketSize(Player player)
    {
        int price = UpgradeMarketSizeCost;
        if (!inventoryManager.EnoughGoldToPurchase(player, price))
        {
            return;
        }
        bool success = marketManager.IncreaseMarketSize();
        if (success)
        {
            inventoryManager.DeductGold(player, price);
        }
    }

    public void TryPurchaseIncreaseArmySize(Player player)
    {
        int price = GetArmySizeCost(player);
        if (!inventoryManager.EnoughGoldToPurchase(player, price))
        {
            return;
        }
        inventoryManager.IncreaseArmySize(player);
        inventoryManager.DeductGold(player, price);
    }

    public int GetArmySizeCost(Player player)
    {
        // based on GameController.cs,
        // https://github.com/next-please/I-was-tasked/blob/8ab8c6782787c40371ab6c65fe04c8f755552a03/Assets/Scripts/GameController.cs
        return inventoryManager.GetArmySize(player);
    }

    public int GetMarketRarityCost()
    {
        // based on GameController.cs,
        // https://github.com/next-please/I-was-tasked/blob/8ab8c6782787c40371ab6c65fe04c8f755552a03/Assets/Scripts/GameController.cs
        return marketManager.GetMarketTier();
    }
}
