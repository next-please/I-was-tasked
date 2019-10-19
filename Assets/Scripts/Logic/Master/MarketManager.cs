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
        if (totalDamage > 0)
        {
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Combat is over! " + totalDamage + " damage done to castle!" });
        }
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    // probably migrate this? - nic
    public int GetCastleHealth()
    {
        return market.GetCastleHealth();
    }

    public List<Piece> GenerateMarketItems()
    {
        if (market.MarketPieces != null)
        {
            characterGenerator.ReturnPieces(market.MarketPieces);
        }
        List<Piece> marketPieces = new List<Piece>();
        for (int i = 0; i < market.GetMarketSize(); ++i)
        {
            Piece piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
            marketPieces.Add(piece);
        }
        return marketPieces;
    }

    public void SetMarketItems(List<Piece> marketPieces)
    {
        this.market.MarketPieces = marketPieces;
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }

    public void RemoveMarketPiece(Piece piece)
    {
        var marketPieces =  market.MarketPieces;
        int index = marketPieces.IndexOf(piece);
        marketPieces.RemoveAt(index);
        marketPieces.Insert(index, null);
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }

    public void IncreaseMarketTier(List<Piece> pieces)
    {
        market.MarketTier++;
        market.MarketPieces = pieces;
        //reactive upgrades
        EventManager.Instance.Raise(new GlobalMessageEvent { message = "Market tier has been upgraded! New mercenaries may be stronger!" });
        EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
    }

    public List<Piece> UpgradePiecesWithTier(int marketTier)
    {
        var marketPieceCopy = new List<Piece>(market.MarketPieces);
        Piece marketPiece;
        for (int i=0; i< marketPieceCopy.Count; i++)
        {
            marketPiece = marketPieceCopy[i];
            if (marketPiece == null)
                continue;
            if (rngesus.Next(1, 101) <= characterGenerator.characterUpgradeDifferencePercentage)
            {
                characterGenerator.TryUpgradeCharacterMarketTier(ref marketPiece, marketTier);
                // EventManager.Instance.Raise(new GlobalMessageEvent { message = market.MarketPieces[i].GetName() + " has grown stronger from the market upgrades!" });
            }
        }
        return marketPieceCopy;
    }

    public int GetMarketTier()
    {
        return market.MarketTier;
    }

    public bool IsMarketFull()
    {
        return market.GetMarketSize() >= market.MaxMarketSize;
    }

    public bool IncreaseMarketSize(Piece piece)
    {
        bool success = market.IncreaseMarketSize();
        if (success)
        {
            //reactive upgrades
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Market space increased! A new mercenary has entered the market." });
            market.MarketPieces.Add(piece);
            EventManager.Instance.Raise(new MarketUpdateEvent{ readOnlyMarket = market });
        }
        return success;
    }

    public Piece GenerateMarketPiece()
    {
        return characterGenerator.GenerateCharacter(market.GetMarketTier());
    }

    public Piece GetActualMarketPiece(Piece piece)
    {
        return market.MarketPieces.Find(marketPiece => piece == marketPiece);
    }
}

public class MarketUpdateEvent : GameEvent
{
    public IReadOnlyMarket readOnlyMarket;
}
