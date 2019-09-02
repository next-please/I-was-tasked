using System;
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
    private List<Piece> bench = new List<Piece>();
    private int armySize = 10;

    public void Reset(int startingGold)
    {
        gold = startingGold;
        bench = new List<Piece>();
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
        return bench.Count >= armySize;
    }

    public bool AddToBench(Piece piece)
    {
        if (IsBenchFull())
            return false;

        bench.Add(piece);
        return true;
    }

    public int GetBenchCount()
    {
        return bench.Count;
    }

    public int GetArmySize()
    {
        return armySize;
    }

    // should be removed soon but for convenience for debugger in board manager
    public List<Piece> GetBench()
    {
        return bench;
    }
}
