using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public int StartingMarketSize = 5;
    public InventoryManager inventoryManager;
    public Market market;
    public CharacterGenerator characterGenerator;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void Awake()
    {
        characterGenerator = new CharacterGenerator();
        market.MarketSize = StartingMarketSize;
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            GenerateMarketItems();
        }
    }

    void GenerateMarketItems()
    {
        if (market.MarketPieces != null)
        {
            characterGenerator.ReturnPieces(market.MarketPieces);
        }
        market.MarketPieces = new List<Piece>();
        for (int i = 0; i < market.MarketSize; ++i)
        {
            Piece piece = characterGenerator.GenerateCharacter(8);
            market.MarketPieces.Add(piece);
            EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
        }
    }

    public void RemoveMarketPiece(Piece piece)
    {
        var marketPieces =  market.MarketPieces;
        int index = marketPieces.IndexOf(piece);
        marketPieces.RemoveAt(index);
        marketPieces.Insert(index, null);
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }

    public void IncreaseMarketTier()
    {
        market.MarketTier++;
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }

    public int GetMarketTier()
    {
        return market.MarketTier;
    }

    public void IncreaseMarketSize()
    {
        market.MarketSize++;
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }
}

public class MarketUpdateEvent : GameEvent
{
    public IReadOnlyMarket readOnlyMarket;
}
