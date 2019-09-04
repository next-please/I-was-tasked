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

    public readonly int numberOfRarityTiers = 4;
    public readonly int[] tiersRacePoolMax = new int[] { 15, 15, 10, 6 };
    public readonly int[] tiersJobPoolMax = new int[] { 15, 15, 10, 6 };
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
    public readonly int[] rarityModifier = { 1, 2, 4, 8 };

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
        for (int i=0; i<tiers.Length; i++)
        {
            tiers[i].RacePoolSize = new int[Enum.GetNames(typeof(Enums.Race)).Length];
            for (int j=0; j<tiers[i].RacePoolSize.Length; j++)
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
        //calculate character rarity
        currentRarityTier = Math.Min(currentRarityTier - 1, rarityUpgradeTiers.GetLength(0));
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
        int raceNumber = rngesus.Next(1, raceTotalPool+1);
        Enums.Race race = 0;
        for (int i=0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++)
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
        int jobNumber = rngesus.Next(1, jobTotalPool+1);
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

        Piece currentPiece = new Piece (
            race.ToString() + " that is a " + job.ToString(),
            defaultHitPoints*rarityModifier[characterRarity],
            defaultAttackDamage*rarityModifier[characterRarity],
            1, // TODO: Please help to verify if this is correct, much thanks~! - Nic
            false);
        currentPiece.SetAttackSpeed(defaultAttackSpeed);
        currentPiece.SetManaPoints(defaultManaPoints);
        currentPiece.SetMovementSpeed(defaultMovementSpeed);
        currentPiece.SetRace(race);
        currentPiece.SetClass(job);
        currentPiece.SetRarity(characterRarity+1);
        return currentPiece;
        //throw not implemented //still need to remove from pool and do stat adjustments
    }

    public void UpdateCharacterPoolFromReturn(Piece returned)
    {
        //throw not implemented
    }
}
