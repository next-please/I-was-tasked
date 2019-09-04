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
    public int MarketSize = 0;
    public int MarketTier = 1; //also used as price for market increase, don't know if here or what

    public IReadOnlyList<Piece> GetMarketPieces()
    {
        return MarketPieces;
    }

    public int GetMarketSize()
    {
        return MarketSize;
    }

    public int GetMarketTier()
    {
        return MarketTier;
    }
}
