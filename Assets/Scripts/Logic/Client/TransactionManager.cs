using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Com.Nextplease.IWT;

public class TransactionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public MarketManager marketManager;
    public RequestHandler requestHandler;
    public BoardManager boardManager;

    public readonly int UpgradeMarketSizeCost = 5;
    public readonly int UpgradeArmySizeArmyMultiplier = 2;
    public readonly int UpgradeArmySizeLinearAdditor = -2;

    public void TryToPurchaseMarketPieceToBench(Player player, Piece piece)
    {
        int price = (int)Math.Pow(2, piece.GetRarity() - 1);
        // check locally if we can do this transaction for immediate feedback
        if (!IsValidPurchase(player, price))
        {
            return;
        }

        // send over the network
        Request req = new Request(ActionTypes.BUY_PIECE, new PieceTransactionData(player, piece, price));
        requestHandler.SendRequest(req);
    }

    public void PurchaseMarketPieceToBench(Player player, Piece piece, int price)
    {
        Piece actualPiece = marketManager.GetActualMarketPiece(piece);
        marketManager.RemoveMarketPiece(actualPiece);
        inventoryManager.AddToBench(player, actualPiece);
        inventoryManager.DeductGold(player, actualPiece.GetPrice());
    }

    public void TrySellBenchPiece(Player player, Piece piece)
    {
        int price = 0; // TODO: currently no money gained from selling?
        Request req = new Request(ActionTypes.SELL_PIECE, new PieceTransactionData(player, piece, price));
        requestHandler.SendRequest(req);
    }

    public void SellBenchPiece(Player player, Piece piece)
    {
        Piece actualPiece = inventoryManager.GetActualBenchPiece(player, piece);
        if (inventoryManager.RemoveFromBench(player, actualPiece))
        {
            SellPiece(player, actualPiece);
        }
    }

    // TODO: Currently no check for sell transaction?
    public bool IsValidSale()
    {
        return true;
    }

#region Sell Board Piece
    public bool CanSellBoardPiece(Player player, Piece piece)
    {
        Piece actualPiece = inventoryManager.GetActualArmyPiece(player, piece);
        return inventoryManager.ArmyContainsPiece(player, actualPiece);
    }

    public void TrySellBoardPiece(Player player, Piece piece)
    {
        if (!CanSellBoardPiece(player, piece))
            return;
        Request req = new Request(ActionTypes.SELL_BOARD_PIECE, new PieceData(player, piece));
        requestHandler.SendRequest(req);
    }

    public void SellBoardPiece(Player player, Piece piece)
    {
        if (!CanSellBoardPiece(player, piece))
            return;
        Piece actualPiece = inventoryManager.GetActualArmyPiece(player, piece);
        boardManager.RemovePieceFromBoard(player, actualPiece);
        inventoryManager.RemoveFromArmy(player, actualPiece);
        SellPiece(player, piece);
    }
#endregion

    // This isn't synced.
    // If you are calling this outside of TransactionManager
    // or ArrangementManager - Please take note!
    // If you want synced behaviour use a method
    // that starts with `Try`
    public void SellPiece(Player player, Piece piece)
    {
        marketManager.characterGenerator.ReturnPiece(piece);
        inventoryManager.AddGold(player, piece.GetPrice());
    }

#region Upgrades

#region Market Rarity
    public bool CanPurchaseIncreaseMarketRarity(Player player)
    {
        int price = GetMarketRarityCost();
        return inventoryManager.EnoughGoldToPurchase(player, price);
    }

    public void TryPurchaseIncreaseMarketRarity(Player player)
    {
        if (!CanPurchaseIncreaseMarketRarity(player))
            return;
        int marketTier = marketManager.GetMarketTier();
        List<Piece> pieces = marketManager.UpgradePiecesWithTier(marketTier + 1);
        UpgradeMarketRarityData data = new UpgradeMarketRarityData(player, pieces);
        Request req = new Request(ActionTypes.UPGRADE_MARKET_RARITY, data);
        requestHandler.SendRequest(req);
    }

    public void PurchaseIncreaseMarketRarity(Player player, List<Piece> pieces)
    {
        int price = GetMarketRarityCost();
        marketManager.IncreaseMarketTier(pieces);
        inventoryManager.DeductGold(player, price);
    }

    public int GetMarketRarityCost()
    {
        // based on GameController.cs,
        // https://github.com/next-please/I-was-tasked/blob/8ab8c6782787c40371ab6c65fe04c8f755552a03/Assets/Scripts/GameController.cs
        return marketManager.GetMarketTier();
    }
#endregion

#region Market Size
    public bool CanPurchaseIncreaseMarketSize(Player player)
    {
        int price = UpgradeMarketSizeCost;
        return inventoryManager.EnoughGoldToPurchase(player, price) &&
               !marketManager.IsMarketFull();
    }

    public void TryPurchaseIncreaseMarketSize(Player player)
    {
        if (!CanPurchaseIncreaseMarketSize(player))
            return;
        Piece piece = marketManager.GenerateMarketPiece();
        UpgradeMarketSizeData data = new UpgradeMarketSizeData(player, piece);
        Request req = new Request(102, data);
        requestHandler.SendRequest(req);
    }

    public void PurchaseIncreaseMarketSize(Player player, Piece piece)
    {
        if (!CanPurchaseIncreaseMarketSize(player))
            return;
        bool success = marketManager.IncreaseMarketSize(piece);
        int price = UpgradeMarketSizeCost;
        if (success)
        {
            inventoryManager.DeductGold(player, price);
        }
    }
#endregion

#region Army Size

    public bool CanPurchaseIncreaseArmySize(Player player)
    {
        int price = GetArmySizeCost(player);
        return inventoryManager.EnoughGoldToPurchase(player, price);
    }

    public void TryPurchaseIncreaseArmySize(Player player)
    {
        UpgradeArmySizeData data = new UpgradeArmySizeData(player);
        Request req = new Request(103, data);
        requestHandler.SendRequest(req);
    }

    public void PurchaseIncreaseArmySize(Player player)
    {
        if (!CanPurchaseIncreaseArmySize(player))
            return;
        int price = GetArmySizeCost(player);
        inventoryManager.IncreaseArmySize(player);
        inventoryManager.DeductGold(player, price);
    }


    public int GetArmySizeCost(Player player)
    {
        // based on GameController.cs,
        // https://github.com/next-please/I-was-tasked/blob/8ab8c6782787c40371ab6c65fe04c8f755552a03/Assets/Scripts/GameController.cs
        return inventoryManager.GetArmySize(player)*UpgradeArmySizeArmyMultiplier + UpgradeArmySizeLinearAdditor;
    }
#endregion

#endregion

    public bool IsValidPurchase(Player player, int price)
    {
        return !inventoryManager.IsBenchFull(player) && inventoryManager.EnoughGoldToPurchase(player, price);
    }
}
