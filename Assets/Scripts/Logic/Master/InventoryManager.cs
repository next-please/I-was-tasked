using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class InventoryManager : MonoBehaviour
{
    private readonly string CLASS_NAME = "InventoryManager";

    public int StartingGold = 0;
    public int StartingArmySize = 2;
    [SerializeField]
    PlayerInventory[] playerInventories;
    public SynergyManager synergyManager;
    public RoomManager roomManager;

    [SerializeField]
    public SynergyTabMenu _synergyTabMenu;

    public void ResetInventories()
    {
        foreach (var p in playerInventories) {
            p.Reset(StartingGold, StartingArmySize);
        }
        synergyManager.Reset();
        _synergyTabMenu.Reset();
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
        var hadJobSynergy = synergyManager.HasSynergy(piece.GetClass());
        var hadRaceSynergy = synergyManager.HasSynergy(piece.GetRace());
        var hadBetterJobSynergy = synergyManager.HasBetterSynergy(piece.GetClass());
        var hadBetterRaceSynergy = synergyManager.HasBetterSynergy(piece.GetRace());
        var success = playerInv.AddToArmy(piece);
        if (success)
        {
            synergyManager.IncreaseSynergyCount(piece.GetClass());
            synergyManager.IncreaseSynergyCount(piece.GetRace());
            Enums.Job pieceClass = piece.GetClass();
            Enums.Race pieceRace = piece.GetRace();
            string classDescription = Enums.JobSynergyDescription[(int)pieceClass];
            string raceDescription = Enums.RaceSynergyDescription[(int)pieceRace];
            _synergyTabMenu.IncrementSynergyTab(pieceClass.ToString(), classDescription, Enums.JobSynergyRequirements[(int)pieceClass]);
            _synergyTabMenu.IncrementSynergyTab(pieceRace.ToString(), raceDescription, Enums.RaceSynergyRequirements[(int)pieceRace]);
            _synergyTabMenu.sortTabs();
            if (synergyManager.HasSynergy(piece.GetClass()) && !hadJobSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Active" });
                EventManager.Instance.Raise(new JobSynergyAppliedEvent{ Job = piece.GetClass(), Applied = true });
            }
            if (!synergyManager.HasSynergy(piece.GetClass()) && hadJobSynergy)
            {
                // NOTE: This is currently only for rogue
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Removed" });
                EventManager.Instance.Raise(new JobSynergyAppliedEvent{ Job = piece.GetClass(), Applied = false });
            }
            if (synergyManager.HasSynergy(piece.GetRace()) && !hadRaceSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Active" });
                EventManager.Instance.Raise(new RaceSynergyAppliedEvent{ Race = piece.GetRace(), Applied = true });
            }
            if (synergyManager.HasBetterSynergy(piece.GetClass()) && !hadBetterJobSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Enhanced" });
                //EventManager.Instance.Raise(new GlobalMessageEvent { message = Enums.JobSynergyDescription[(int)piece.GetClass()] });
            }
            if (synergyManager.HasBetterSynergy(piece.GetRace()) && !hadBetterRaceSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Enhanced" });
                //EventManager.Instance.Raise(new GlobalMessageEvent { message = Enums.RaceSynergyDescription[(int)piece.GetRace()] });
            }
            EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
        }
        return success;
    }

    public bool RemoveFromArmy(Player player, Piece piece)
    {
        var playerInv = GetPlayerInventory(player);
        var hadJobSynergy = synergyManager.HasSynergy(piece.GetClass());
        var hadRaceSynergy = synergyManager.HasSynergy(piece.GetRace());
        var hadBetterJobSynergy = synergyManager.HasBetterSynergy(piece.GetClass());
        var hadBetterRaceSynergy = synergyManager.HasBetterSynergy(piece.GetRace());
        var success = playerInv.RemoveFromArmy(piece);
        if (success)
        {
            synergyManager.DecreaseSynergyCount(piece.GetClass());
            synergyManager.DecreaseSynergyCount(piece.GetRace());
            Enums.Job pieceClass = piece.GetClass();
            Enums.Race pieceRace = piece.GetRace();
            _synergyTabMenu.DecrementSynergyTab(pieceClass.ToString());
            _synergyTabMenu.DecrementSynergyTab(pieceRace.ToString());
            _synergyTabMenu.sortTabs();
            if (!synergyManager.HasSynergy(piece.GetClass()) && hadJobSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Removed" });
                EventManager.Instance.Raise(new JobSynergyAppliedEvent{ Job = piece.GetClass(), Applied = false });
            }
            if (synergyManager.HasSynergy(piece.GetClass()) && !hadJobSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Active" });
                EventManager.Instance.Raise(new JobSynergyAppliedEvent{ Job = piece.GetClass(), Applied = true });
            }
            if (!synergyManager.HasSynergy(piece.GetRace()) && hadRaceSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Removed" });
                EventManager.Instance.Raise(new RaceSynergyAppliedEvent{ Race = piece.GetRace(), Applied = false });
            }
            if (!synergyManager.HasBetterSynergy(piece.GetClass()) && hadBetterJobSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetClass() + " Synergy Weakened" });
            }
            if (!synergyManager.HasBetterSynergy(piece.GetRace()) && hadBetterRaceSynergy)
            {
                EventManager.Instance.Raise(new GlobalMessageEvent { message = piece.GetRace() + " Synergy Weakened" });
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

    // use for tutorial, please use carefully
    public void SetGold(Player player, int amount)
    {
        var playerInv = GetPlayerInventory(player);
        playerInv.SetGold(amount);
        EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = playerInv });
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

    public int[] GetAllPlayerGold()
    {
        int[] gold = new int[playerInventories.Length];
        for(int i = 0; i < playerInventories.Length; i++)
        {
            gold[i] = playerInventories[i].GetGold();
        }
        return gold;
    }

    public void CheckAndSetAllPlayerGold(int[] gold)
    {
        for (int i = 0; i < playerInventories.Length; i++)
            if (playerInventories[i].GetGold() != gold[i])
            {
                Debug.LogFormat("{0}: Player {1}'s gold is not properly calculated, force setting gold...", CLASS_NAME, i);
                SetGold((Player)i, gold[i]);
            }
     }
}

public class InventoryChangeEvent : GameEvent
{
    public PlayerInventory inventory;
}

