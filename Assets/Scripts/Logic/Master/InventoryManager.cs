using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int StartingGold = 0;
    public int StartingArmySize = 1;
    [SerializeField]
    PlayerInventory[] playerInventories;
    public SynergyManager synergyManager;

    public void ResetInventories()
    {
        foreach (var p in playerInventories)
        {
            p.Reset(StartingGold, StartingArmySize);
        }
        synergyManager.Reset();
    }

    public PlayerInventory GetPlayerInventory(Player player)
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

    public bool IsBenchSlotVacant(Player player, int slotIndex)
    {
        return GetPlayerInventory(player).BenchVacantAtIndex(slotIndex);
    }

    public bool AddToBench(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        bool success = playerInv.AddToBench(piece);
        if (success)
        {
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
        return success;
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

    public bool ArmyContainsPiece(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.ArmyContainsPiece(piece);
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

    public bool IsArmyFull(Player player)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.IsArmyFull();
    }

    public bool AddToArmy(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        var hadJobSynergy = playerInv.HasSynergy(piece.GetClass());
        var hadRaceSynergy = playerInv.HasSynergy(piece.GetRace());
        var success = playerInv.AddToArmy(piece);
        if (success)
        {
            if (playerInv.HasSynergy(piece.GetClass()) && !hadJobSynergy)
            {
                synergyManager.IncreaseSynergyCount(piece.GetClass());
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Active" });
                EventManager.Instance.Raise(new GlobalMessageEvent { message = Enums.JobSynergyDescription[(int)piece.GetClass()] });
            }
            if (playerInv.HasSynergy(piece.GetRace()) && !hadRaceSynergy)
            {
                synergyManager.IncreaseSynergyCount(piece.GetRace());
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Active" });
                EventManager.Instance.Raise(new GlobalMessageEvent { message = Enums.RaceSynergyDescription[(int)piece.GetRace()] });
            }
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
        return success;
    }

    public bool RemoveFromArmy(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        var hadJobSynergy = playerInv.HasSynergy(piece.GetClass());
        var hadRaceSynergy = playerInv.HasSynergy(piece.GetRace());
        var success = playerInv.RemoveFromArmy(piece);
        if (success)
        {
            if (!playerInv.HasSynergy(piece.GetClass()) && hadJobSynergy)
            {
                synergyManager.DecreaseSynergyCount(piece.GetClass());
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Removed" });
            }
            if (!playerInv.HasSynergy(piece.GetRace()) && hadRaceSynergy)
            {
                synergyManager.DecreaseSynergyCount(piece.GetRace());
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Removed" });
            }
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
        return success;
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

    public List<Piece> GetExcessPieces(Player player)
    {
        var playerInv = GetPlayerInventory(player);
        int size = playerInv.GetArmySize();
        return playerInv.GetExcessArmyPieces();
    }

    public Piece GetActualArmyPiece(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.GetActualArmyPiece(piece);
    }

    public Piece GetActualBenchPiece(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        return playerInv.GetBenchPiece(piece);
    }
}

public class InventoryChangeEvent : GameEvent
{
    public PlayerInventory inventory;
}

