using System;
using System.Collections.ObjectModel;
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
    private int armySize = 4;
    private static int MaxBenchSize = 8;

    public void Reset(int startingGold)
    {
        gold = startingGold;
        bench = new Piece[MaxBenchSize];
        benchCount = 0;
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

    public int GetArmySize()
    {
        return armySize;
    }

    public void IncreaseArmySize()
    {
        armySize++;
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
}
