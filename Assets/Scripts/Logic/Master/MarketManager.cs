using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public int StartingMarketSize = 5;
    public int StartingMarketTier = 1;
    public int StartingCastleHealth = 10;
    public InventoryManager inventoryManager;
    public Market market;
    public CharacterGenerator characterGenerator;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.AddListener<OnDamageEvent>(OnDamageEvent);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void Awake()
    {
        characterGenerator = new CharacterGenerator();
        market.SetMarketSize(StartingMarketSize);
        market.MarketTier = StartingMarketTier;
        market.CastleHealth = StartingCastleHealth;
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            GenerateMarketItems();
        }
    }

    void OnDamageEvent(OnDamageEvent e)
    {
        market.CastleHealth -= e.damage;
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    void GenerateMarketItems()
    {
        if (market.MarketPieces != null)
        {
            characterGenerator.ReturnPieces(market.MarketPieces);
        }
        market.MarketPieces = new List<Piece>();
        for (int i = 0; i < market.GetMarketSize(); ++i)
        {
            Piece piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
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

    public bool IncreaseMarketSize()
    {
        bool success = market.IncreaseMarketSize();
        if (success)
        {
            EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
        }
        return success;
    }
}

public class MarketUpdateEvent : GameEvent
{
    public IReadOnlyMarket readOnlyMarket;
}
