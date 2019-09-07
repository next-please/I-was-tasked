using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    Zero = 0,
    One = 1,
    Two = 2,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerInventory", order = 1)]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]
    private Player owner;
    [SerializeField]
    private int gold;
    private Piece[] bench; // bench represented as an array with potential nulls for rearrangement
    private int benchCount = 0; // number of pieces on the bench
    private List<Piece> army; // pieces on board
    private int armySize; // max pieces on board
    private static int MaxBenchSize = 8;

    public void Reset(int startingGold, int startingArmySize)
    {
        gold = startingGold;
        bench = new Piece[MaxBenchSize];
        army = new List<Piece>();
        benchCount = 0;
        armySize = startingArmySize;
    }

    public Player GetOwner()
    {
        return owner;
    }

    public int GetGold()
    {
        return gold;
    }

    public bool CanPurchase(float price)
    {
        return gold - price >= 0;
    }

    public bool DeductGold(int amount)
    {
        if (gold - amount < 0)
            return false;
        gold -= amount;
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public bool IsBenchFull()
    {
        return benchCount >= MaxBenchSize;
    }

    public bool AddToBench(Piece piece)
    {
        if (IsBenchFull())
            return false;

        for (int i = 0; i < bench.Length; ++i)
        {
            Piece benchPiece = bench[i];
            if (benchPiece == null)
            {
                bench[i] = piece;
                benchCount++;
                break;
            }

        }
        return true;
    }

    public int GetBenchCount()
    {
        return benchCount;
    }

    public bool BenchContainsPiece(Piece piece)
    {
        for (int i = 0; i < bench.Length; ++i)
        {
            Piece benchPiece = bench[i];
            if (benchPiece == piece)
                return true;
        }
        return false;
    }

    public bool RemovePieceFromBench(Piece piece)
    {
        for (int i = 0; i < bench.Length; ++i)
        {
            Piece benchPiece = bench[i];
            if (benchPiece == piece)
            {
                bench[i] = null;
                benchCount--;
                return true;
            }
        }
        return false;
    }

    public bool BenchVacantAtIndex(int index)
    {
        if (index <= 0 || index >= GetBenchCount())
        {
            return false;
        }
        return bench[index] == null;
    }

    public int GetIndexOfBenchPiece(Piece piece)
    {
        for (int i = 0; i < bench.Length; ++i)
        {
            Piece benchPiece = bench[i];
            if (benchPiece == piece)
                return i;
        }
        return -1;
    }

    public void SetBenchPieceAtIndex(Piece piece, int index)
    {
        bench[index] = piece;
    }

    public ReadOnlyCollection<Piece> GetBench()
    {
        return Array.AsReadOnly(bench);
    }

   public int GetArmySize()
    {
        return armySize;
    }

    public void IncreaseArmySize()
    {
        armySize++;
    }

    public bool AddToArmy(Piece piece)
    {
        army.Add(piece);
        return true;
    }

    public bool RemoveFromArmy(Piece piece)
    {
        return army.Remove(piece);
    }

    public bool IsArmyFull()
    {
        return army.Count >= armySize;
    }

    public int GetArmyCount()
    {
        return army.Count;
    }

    public List<Piece> GetExcessArmyPieces()
    {
        int excessCount = GetArmyCount() - GetArmySize();
        return army.Skip(GetArmySize()).Take(excessCount).ToList();
    }
}
