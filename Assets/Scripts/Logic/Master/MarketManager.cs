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
    private System.Random rngesus = new System.Random();

    void Awake()
    {
        characterGenerator = new CharacterGenerator();
        market.SetMarketSize(StartingMarketSize);
        market.MarketTier = StartingMarketTier;
        market.CastleHealth = StartingCastleHealth;
    }

    public void CalculateAndApplyDamageToCastle(List<Piece> piecesOnBoard)
    {
        int totalDamage = 0;
        foreach (Piece piece in piecesOnBoard)
        {
            totalDamage += piece.GetDamageIfSurvive();
        }
        market.CastleHealth -= totalDamage;
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    // probably migrate this? - nic
    public int GetCastleHealth()
    {
        return market.GetCastleHealth();
    }

    public void GenerateMarketItems()
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

        //reactive upgrades
        EventManager.Instance.Raise(new GlobalMessageEvent { message = "Market tier has been upgraded! New mercenaries may be stronger!" });
        for (int i = 0; i < market.GetMarketSize(); ++i)
        {
            if (market.MarketPieces[i] != null)
            {
                if (rngesus.Next(1, 101) <= characterGenerator.characterUpgradeDifferencePercentage)
                {
                    if (characterGenerator.TryUpgradeCharacter(market.MarketPieces[i], market.MarketTier))
                    {
                        EventManager.Instance.Raise(new GlobalMessageEvent { message = market.MarketPieces[i].GetName() + " has grown stronger from the market upgrades!" });
                    }
                }
            }
        }

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
            //reactive upgrades
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Market space increased! A new mercenary has entered the market." });
            Piece piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
            market.MarketPieces.Add(piece);

            EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
        }
        return success;
    }
}

public class MarketUpdateEvent : GameEvent
{
    public IReadOnlyMarket readOnlyMarket;
}
