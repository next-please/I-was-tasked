using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public int StartingMarketSize = 5;
    public int StartingMarketTier = 1;
    public int StartingCastleHealth = 15;
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

    public int GetMarketUpgradeCost(int currentMarketTier)
    {
        switch (currentMarketTier)
        {
            case 1:
                return 3;
            case 2:
                return 6;
            case 3:
                return 9;
            case 4:
                return 12;
            default:
                return 99;
        }

    }

    public bool CalculateAndApplyDamageToCastle(List<Piece> piecesOnBoard)
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
            EventManager.Instance.Raise(new DamageTakenEvent { currentHealth = market.CastleHealth });
        }
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
        return totalDamage > 0;
    }

    // probably migrate this? - nic
    public int GetCastleHealth()
    {
        return market.GetCastleHealth();
    }

    public bool CalculateAndApplyDamageToCastle(int updatedHealth)
    {
        if (updatedHealth == market.GetCastleHealth())
            return false;

        int totalDamage = market.GetCastleHealth() - updatedHealth;
        market.CastleHealth -= totalDamage;
        if(totalDamage > 0)
        {
            EventManager.Instance.Raise(new GlobalMessageEvent { message = "Combat is over! " + totalDamage + " damage done to castle!" });
            EventManager.Instance.Raise(new DamageTakenEvent { currentHealth = market.CastleHealth });
        }
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
        return true;
    }

    public List<Piece> GenerateMarketItems()
    {
        bool orcCountry = inventoryManager.synergyManager.HasBetterSynergy(Enums.Race.Orc);

        if (market.MarketPieces != null)
        {
            characterGenerator.ReturnPieces(market.MarketPieces);
        }
        List<Piece> marketPieces = new List<Piece>();
        for (int i = 0; i < market.GetMarketSize(); ++i)
        {
            Piece piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
            if (orcCountry)
            {
                while (piece.GetRace() != Enums.Race.Orc)
                {
                    piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
                }
            }
            marketPieces.Add(piece);
        }
        return marketPieces;
    }

    public void SetMarketItems(List<Piece> marketPieces)
    {
        this.market.MarketPieces = marketPieces;
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    public void RemoveMarketPiece(Piece piece)
    {
        var marketPieces = market.MarketPieces;
        int index = marketPieces.IndexOf(piece);
        marketPieces.RemoveAt(index);
        marketPieces.Insert(index, null);
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    public void IncreaseMarketTier(List<Piece> pieces)
    {
        market.MarketTier++;
        market.MarketPieces = pieces;
        //reactive upgrades
        EventManager.Instance.Raise(new GlobalMessageEvent { message = "Market tier has been upgraded! New mercenaries may be stronger!" });
        EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
    }

    public List<Piece> UpgradePiecesWithTier(int marketTier)
    {
        var marketPieceCopy = new List<Piece>(market.MarketPieces);
        return marketPieceCopy;
        //No more reactive upgrades for market
        Piece marketPiece;
        for (int i = 0; i < marketPieceCopy.Count; i++)
        {
            marketPiece = marketPieceCopy[i];
            if (marketPiece == null)
                continue;
            if (rngesus.Next(1, 101) <= characterGenerator.characterUpgradeDifferencePercentage)
            {
                characterGenerator.TryUpgradeCharacterMarketTier(ref marketPiece, marketTier);
                EventManager.Instance.Raise(new GlobalMessageEvent { message = market.MarketPieces[i].GetName() + " has grown stronger from the market upgrades!" });
                marketPieceCopy[i] = marketPiece;
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
            EventManager.Instance.Raise(new MarketUpdateEvent { readOnlyMarket = market });
        }
        return success;
    }

    public Piece GenerateMarketPiece()
    {
        bool orcCountry = inventoryManager.synergyManager.HasBetterSynergy(Enums.Race.Orc);
        Piece piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
        if (orcCountry)
        {
            while (piece.GetRace() != Enums.Race.Orc)
            {
                piece = characterGenerator.GenerateCharacter(market.GetMarketTier());
            }
        }
        return piece;
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

public class DamageTakenEvent : GameEvent
{
    public int currentHealth;
}
