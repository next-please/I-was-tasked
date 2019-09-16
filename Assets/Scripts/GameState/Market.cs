using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyMarket
{
    IReadOnlyList<Piece> GetMarketPieces();
    int GetMarketSize();
    int GetMarketTier();
    int GetCastleHealth();
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Market", order = 1)]
public class Market : ScriptableObject, IReadOnlyMarket
{
    public List<Piece> MarketPieces;
    private int marketSize = 0;
    public int MarketTier = 1; //also used as price for market increase, don't know if here or what
    public int CastleHealth = 10;
    public readonly int MaxMarketSize = 12;

    void OnEnable()
    {
        MarketPieces = new List<Piece>();
    }

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

    public int GetCastleHealth()
    {
        return CastleHealth;
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
