using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Com.Nextplease.IWT;

public class TransactionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public MarketManager marketManager;
    public IncomeManager incomeManager;
    public RequestHandler requestHandler;


    public readonly int UpgradeIncomeCost = 10;
    public readonly int UpgradeMarketSizeCost = 10;

    public void TryToPurchaseMarketPieceToBench(Player player, Piece piece)
    {
        int price = (int)Math.Pow(2, piece.GetRarity() - 1);

        // check locally if we can do this transaction for immediate feedback
        if (!IsValidPurchase(player, price))
        {
            return;
        }

        // send over the network
        Request req = new Request(5, new PieceTransactionData(player, piece, price));
        requestHandler.SendRequest(req);
    }

    public void PurchaseMarketPieceToBench(Player player, Piece piece, int price)
    {
        marketManager.RemoveMarketPiece(piece);
        inventoryManager.AddToBench(player, piece);
        inventoryManager.DeductGold(player, price);
    }

    public void TrySellBenchPiece(Player player, Piece piece)
    {
        int price = 0; // TODO: currently no money gained from selling?
        Request req = new Request(6, new PieceTransactionData(player, piece, price));
        requestHandler.SendRequest(req);
    }

    // TODO: Currently no check for sell transaction?
    public bool IsValidSale()
    {
        return true;
    }

    public void SellBenchPiece(Player player, Piece piece)
    {
        if (inventoryManager.RemoveFromBench(player, piece))
        {
            marketManager.characterGenerator.ReturnPiece(piece);
        }
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

    public bool IsValidPurchase(Player player, int price)
    {
        return !inventoryManager.IsBenchFull(player) && inventoryManager.EnoughGoldToPurchase(player, price);
    }
}
