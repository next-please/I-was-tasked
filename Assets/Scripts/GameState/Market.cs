using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyMarket
{
    IReadOnlyList<Piece> GetMarketPieces();
    int GetMarketSize();
    int GetMarketTier();
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Market", order = 1)]
public class Market : ScriptableObject, IReadOnlyMarket
{
    public List<Piece> MarketPieces;
    private int marketSize = 0;
    public int MarketTier = 1; //also used as price for market increase, don't know if here or what
    public readonly int MaxMarketSize = 12;

    public IReadOnlyList<Piece> GetMarketPieces()
    {
        return MarketPieces;
    }

    public int GetMarketSize()
    {
        return marketSize;
    }

    public int GetMarketTier()
    {
        return MarketTier;
    }

    public bool IncreaseMarketSize()
    {
        if (marketSize >= MaxMarketSize)
        {
            return false;
        }
        marketSize++;
        return true;
    }

    public void SetMarketSize(int size)
    {
        marketSize = size;
    }
}
