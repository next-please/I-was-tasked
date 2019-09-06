using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int StartingGold = 0;
    public int StartingArmySize = 1;
    [SerializeField]
    PlayerInventory[] playerInventories;

    public void ResetInventories()
    {
        foreach (var p in playerInventories)
        {
            p.Reset(StartingGold, StartingArmySize);
        }
    }

    PlayerInventory GetPlayerInventory(Player player)
    {
        int index = (int) player;
        return playerInventories[index];
    }

    public bool EnoughGoldToPurchase(Player player, int price)
    {
        return GetPlayerInventory(player).CanPurchase(price);
    }

    public bool IsBenchFull(Player player)
    {
        return GetPlayerInventory(player).IsBenchFull();
    }

    public void AddToBench(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        bool success = playerInv.AddToBench(piece);
        if (success)
        {
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
    }

    public bool RemoveFromBench(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        bool success = playerInv.RemovePieceFromBench(piece);
        if (success)
        {
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
        return success;
    }

    public bool BenchContainsPiece(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.BenchContainsPiece(piece);
    }

    public int GetGold(Player player)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.GetGold();
    }

    public void AddGold(Player player, int amount)
    {
        var playerInv = GetPlayerInventory(player);
        playerInv.AddGold(amount);
        EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
    }

    public void DeductGold(Player player, int amount)
    {
        var playerInv = GetPlayerInventory(player);
        bool success  = playerInv.DeductGold(amount);
        if (success)
        {
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
    }

    public void MoveBenchPieceToIndex(Player player, Piece piece, int index)
    {
        var playerInv = GetPlayerInventory(player);
        int originalIndex = playerInv.GetIndexOfBenchPiece(piece);
        if (originalIndex < 0)
            return;
        playerInv.SetBenchPieceAtIndex(null, originalIndex);
        playerInv.SetBenchPieceAtIndex(piece, index);
        EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
    }

    public void IncreaseArmySize(Player player)
    {
        var playerInv = GetPlayerInventory(player);
        playerInv.IncreaseArmySize();
        EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
    }

    public int GetArmySize(Player player)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.GetArmySize();
    }
}

public class InventoryChangeEvent : GameEvent
{
    public PlayerInventory inventory;
}

