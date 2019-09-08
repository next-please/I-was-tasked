using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterGenerator
{
    private string name;
    private System.Random rngesus;

    public readonly int defaultHitPoints = 100;
    public readonly int defaultManaPoints = 100;
    public readonly int defaultAttackDamage = 10;
    public readonly int defaultAttackRange = 1;
    public readonly int defaultAttackSpeed = 5;
    public readonly int minimumAttackSpeed = 1;
    public readonly int maximumAttackSpeed = 10;
    public readonly int defaultMovementSpeed = 5;
    public readonly int minimumMovementSpeed = 1;
    public readonly int maximumMovementSpeed = 10;
    public readonly double hitPointMultiplier = 1.5;
    public readonly double attackDamageMultiplier = 1.5;
    public readonly int rangeAdditor = 2;
    public readonly int movementVariationRange = 2;

    public readonly int numberOfRarityTiers = 4;
    public readonly int[] tiersRacePoolMax = new int[] { 15, 15, 10, 6 };
    public readonly int[] tiersJobPoolMax = new int[] { 15, 15, 10, 6 };
    public readonly int characterUpgradeDifferencePercentage = 20;
    public readonly int[,] rarityUpgradeTiers = {
                                            { 100, 0, 0, 0 },
                                            { 90, 10, 0, 0 },
                                            { 80, 20, 0, 0 },
                                            { 70, 30, 0, 0 },
                                            { 70, 20, 10, 0 },
                                            { 60, 30, 10, 0 },
                                            { 50, 40, 10, 0 },
                                            { 50, 30, 20, 0 },
                                            { 40, 40, 20, 0 },
                                            { 40, 30, 30, 0 },
                                            { 40, 30, 20, 10 },
                                            { 30, 40, 20, 10 },
                                            { 30, 30, 30, 10 },
                                            { 20, 40, 30, 10 },
                                            { 10, 50, 30, 10 },
                                            { 10, 40, 40, 10 },
                                            { 10, 40, 30, 20 },
                                            { 0, 50, 30, 20 },
                                            { 0, 40, 40, 20 },
                                            { 0, 40, 30, 30 },
                                            { 0, 30, 40, 30 },
                                            { 0, 30, 30, 40 },
                                            { 0, 20, 40, 40 }
                                        };
    public readonly int[] rarityBonusUpgrades = new int[] { 0, 3, 6, 9 };
    private Tier[] tiers;

    public struct Tier
    {
        public int RacePoolMax;
        public int JobPoolMax;
        public int[] RacePoolSize;
        public int[] JobPoolSize;
    }

    // Constructor
    public CharacterGenerator()
    {
        rngesus = new System.Random();

        //creating pools for race and job limits
        tiers = new Tier[numberOfRarityTiers];
        for (int i = 0; i < tiers.Length; i++)
        {
            tiers[i].RacePoolSize = new int[Enum.GetNames(typeof(Enums.Race)).Length];
            for (int j = 0; j < tiers[i].RacePoolSize.Length; j++)
            {
                tiers[i].RacePoolSize[j] = tiersRacePoolMax[i];
            }
            tiers[i].JobPoolSize = new int[Enum.GetNames(typeof(Enums.Job)).Length];
            for (int j = 0; j < tiers[i].JobPoolSize.Length; j++)
            {
                tiers[i].JobPoolSize[j] = tiersJobPoolMax[i];
            }
        }
    }

    public Piece GenerateCharacter(int currentRarityTier)
    {
        int currentHitPoints = defaultHitPoints;
        int currentManaPoints = defaultManaPoints;
        int currentAttackDamage = defaultAttackDamage;
        int currentAttackRange = defaultAttackRange;
        int currentAttackSpeed = defaultAttackSpeed;
        int currentMovementSpeed = defaultMovementSpeed;

        //calculate character rarity
        currentRarityTier = Math.Min(currentRarityTier - 1, rarityUpgradeTiers.GetLength(0) - 1);
        int rarityTotalPool = 0;
        for (int i = 0; i < numberOfRarityTiers; i++)
        {
            rarityTotalPool += rarityUpgradeTiers[currentRarityTier, i];
        }
        int rarityNumber = rngesus.Next(1, rarityTotalPool);
        int characterRarity = 0;
        for (int i = 0; i < numberOfRarityTiers; i++)
        {
            rarityNumber -= rarityUpgradeTiers[currentRarityTier, i];
            if (rarityNumber <= 0)
            {
                characterRarity = i;
                break;
            }
        }

        //calculate race
        int raceTotalPool = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++)
        {
            raceTotalPool += tiers[characterRarity].RacePoolSize[i];
        }
        int raceNumber = rngesus.Next(1, raceTotalPool + 1);
        Enums.Race race = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++)
        {
            raceNumber -= tiers[characterRarity].RacePoolSize[i];
            if (raceNumber <= 0)
            {
                race = (Enums.Race)i;
                break;
            }
        }

        //calculate job
        int jobTotalPool = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Job)).Length; i++)
        {
            jobTotalPool += tiers[characterRarity].JobPoolSize[i];
        }
        int jobNumber = rngesus.Next(1, jobTotalPool + 1);
        Enums.Job job = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++)
        {
            jobNumber -= tiers[characterRarity].JobPoolSize[i];
            if (jobNumber <= 0)
            {
                job = (Enums.Job)i;
                break;
            }
        }

        //randomize movement
        int randomValue = rngesus.Next(-movementVariationRange, movementVariationRange + 1);
        currentMovementSpeed += randomValue;

        //calculate stats
        for (int i = 0; i < rarityBonusUpgrades[characterRarity]; i++)
        {
            randomValue = rngesus.Next(1, 3);
            switch (randomValue)
            {
                case 1:
                    currentHitPoints = (int)Math.Floor(currentHitPoints * hitPointMultiplier);
                    break;
                case 2:
                    currentAttackDamage = (int)Math.Floor(currentAttackDamage * attackDamageMultiplier);
                    break;
                default:
                    break;
            }
        }

        //randomize range unit
        bool rangeUpgrade = false;
        do
        {
            randomValue = rngesus.Next(0, 11);
            if (randomValue < 3)
            {
                rangeUpgrade = true;
                currentAttackRange += rangeAdditor;

                //decrease another stat to compensate
                randomValue = rngesus.Next(1, 3);
                switch (randomValue)
                {
                    case 1:
                        currentHitPoints = (int)Math.Ceiling(currentHitPoints / hitPointMultiplier);
                        break;
                    case 2:
                        currentAttackDamage = (int)Math.Ceiling(currentAttackDamage / attackDamageMultiplier);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                rangeUpgrade = false;
            }
        } while (rangeUpgrade);

        tiers[characterRarity].JobPoolSize[(int)job]--;
        tiers[characterRarity].RacePoolSize[(int)race]--;
        Piece currentPiece = new Piece(
            NameGenerator.GenerateName(job, race),
            currentHitPoints,
            currentAttackDamage,
            currentAttackRange, // TODO: Please help to verify if this is correct, much thanks~! - Nic
            false);
        currentPiece.SetAttackSpeed(currentAttackSpeed);
        currentPiece.SetMaximumManaPoints(currentManaPoints);
        currentPiece.SetMovementSpeed(currentMovementSpeed);
        currentPiece.SetRace(race);
        currentPiece.SetClass(job);
        currentPiece.SetRarity(characterRarity + 1);
        return currentPiece;
        //throw not implemented //still need to remove from pool and do stat adjustments
    }

    public void ReturnPiece(Piece piece)
    {
        int rarityTier = piece.GetRarity() - 1;
        tiers[rarityTier].JobPoolSize[(int)piece.GetClass()]++;
        tiers[rarityTier].RacePoolSize[(int)piece.GetRace()]++;
    }

    public void ReturnPieces(List<Piece> returnedPieces)
    {
        foreach (Piece piece in returnedPieces)
        {
            if (piece != null)
            {
                ReturnPiece(piece);
            }
        }
    }

    public bool TryUpgradeCharacter(Piece piece, int currentMarketTier)
    {
        //if character rarity can be found in the new market tier
        if (rarityUpgradeTiers[currentMarketTier - 1, piece.GetRarity()] > 0)
        {
            piece.SetRarity(piece.GetRarity() + 1);

            int randomValue = 0;
            for (int i = rarityBonusUpgrades[piece.GetRarity() - 1]; i < rarityBonusUpgrades[piece.GetRarity()]; i++)
            {
                randomValue = rngesus.Next(1, 3);
                switch (randomValue)
                {
                    case 1:
                        piece.SetMaximumHitPoints((int)Math.Floor(piece.GetMaximumHitPoints() * hitPointMultiplier));
                        break;
                    case 2:
                        piece.SetAttackDamage((int)Math.Floor(piece.GetAttackDamage() * attackDamageMultiplier));
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        return false;
    }
}
