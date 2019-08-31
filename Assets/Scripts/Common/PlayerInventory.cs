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
    private int StartingGold = 999;
    [SerializeField]
    private Player owner;
    [SerializeField]
    private int gold;
    private List<Piece> garrison = new List<Piece>();
    private int armySize = 10;

    void OnEnable()
    {
        gold = StartingGold;
        garrison = new List<Piece>();
    }

    public Player GetOwner()
    {
        return owner;
    }

    public int GetGold()
    {
        return gold;
    }

    public bool TryToPurchase(int amount)
    {
        if (gold - amount < 0)
            return false;
        gold -= amount;
        RaiseInventoryChangeEvent();
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        RaiseInventoryChangeEvent();
    }

    public bool IsGarrisonFull()
    {
        return garrison.Count > armySize;
    }

    public bool AddToGarrison(Piece piece)
    {
        if (IsGarrisonFull())
            return false;

        garrison.Add(piece);
        RaiseInventoryChangeEvent();
        return true;
    }

    public int GetGarrisonCount()
    {
        return garrison.Count;
    }

    public int GetArmySize()
    {
        return armySize;
    }

    // should be removed soon but for convenience for debugger in board manager
    public List<Piece> GetGarrison()
    {
        return garrison;
    }

    void RaiseInventoryChangeEvent()
    {
        EventManager.Instance.Raise(new InventoryChangeEvent{ inventory = this });
    }
}

public class InventoryChangeEvent : GameEvent
{
    public PlayerInventory inventory;
}
