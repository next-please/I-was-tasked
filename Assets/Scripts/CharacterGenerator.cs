using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterGenerator
{
    private string name;
    private System.Random rngesus;

    public readonly int defaultHitPoints = 150;
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

    public readonly double humanPriestManaMultiplier = 1.3;

    //human stat changes
    public readonly double humanHitPointMultiplier = 1.1;
    public readonly double humanAttackDamageMultiplier = 1.1;
    public readonly int humanManaPointAdditor = -5;

    //orc stat changes
    public readonly int orcFlatMovementSpeedAdditor = -1;
    public readonly double orcHitPointMultiplier = 1.25;
    public readonly double orcAttackDamageMultiplier = 1.25;
    public readonly int orcFlatAttackSpeedAdditor = -1;

    //elf stat changes
    public readonly int elfFlatMovementSpeedAdditor = 1;
    public readonly int elfFlatAttackSpeedAdditor = 1;
    public readonly int elfFlatConditionalAttackRangeAdditor = 1;
    public readonly double elfAttackDamageMultiplier = 1.05;
    public readonly int elfManaPointAdditor = -8;

    //undead stat changes
    public readonly int undeadFlatMovementSpeedAdditor = -3;
    public readonly int undeadFlatAttackSpeedAdditor = -2;
    public readonly double undeadHitPointMultiplier = 1.35;

    //mage stat changes
    public readonly int mageFlatAttackRangeAdditor = 2;
    public readonly int mageManaPointAdditor = -8;
    public readonly double mageAttackDamageMultiplier = 1.05;

    //rogue stat changes
    public readonly int rogueFlatMovementSpeedAdditor = 1;
    public readonly int rogueFlatAttackSpeedAdditor = 1;
    public readonly double rogueHitPointMultiplier = 1.05;
    public readonly double rogueAttackDamageMultiplier = 1.1;

    //druid stat changes
    public readonly double druidHitPointMultiplier = 1.15;
    public readonly double druidAttackDamageMultiplier = 1.15;
    public readonly int druidManaPointAdditor = -3;

    //knight stat changes
    public readonly double knightHitPointMultiplier = 1.2;
    public readonly double knightAttackDamageMultiplier = 1.15;

    //priest stat changes
    public readonly int priestFlatAttackRangeAdditor = 4;
    public readonly int priestManaPointAdditor = -5;
    public readonly double priestHitPointMultiplier = 1.1;

    public readonly int[,,] tiersRaceJobPoolMax = {
        {
            { 4, 4, 4, 4, 4 },
            { 4, 4, 4, 4, 4 },
            { 4, 4, 4, 4, 4 },
            { 4, 4, 4, 4, 4 }
        },
        {
            { 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3 },
            { 3, 3, 3, 3, 3 }
        },
        {
            { 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2 },
            { 2, 2, 2, 2, 2 }
        },
        {
            { 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1 }
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        }
    };

    public readonly int characterUpgradeDifferencePercentage = 20;
    public readonly int[,] rarityUpgradeTiers = {
                                            { 100, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 90, 10, 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 80, 20, 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 70, 30, 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 70, 20, 10, 0, 0, 0, 0, 0, 0, 0 },
                                            { 60, 30, 10, 0, 0, 0, 0, 0, 0, 0 },
                                            { 50, 40, 10, 0, 0, 0, 0, 0, 0, 0 },
                                            { 50, 30, 20, 0, 0, 0, 0, 0, 0, 0 },
                                            { 40, 40, 20, 0, 0, 0, 0, 0, 0, 0 },
                                            { 40, 30, 30, 0, 0, 0, 0, 0, 0, 0 },
                                            { 40, 30, 20, 10, 0, 0, 0, 0, 0, 0 },
                                            { 30, 40, 20, 10, 0, 0, 0, 0, 0, 0 },
                                            { 30, 30, 30, 10, 0, 0, 0, 0, 0, 0 },
                                            { 20, 40, 30, 10, 0, 0, 0, 0, 0, 0 },
                                            { 10, 50, 30, 10, 0, 0, 0, 0, 0, 0 },
                                            { 10, 40, 40, 10, 0, 0, 0, 0, 0, 0 },
                                            { 10, 40, 30, 20, 0, 0, 0, 0, 0, 0 },
                                            { 0, 50, 30, 20, 0, 0, 0, 0, 0, 0 },
                                            { 0, 40, 40, 20, 0, 0, 0, 0, 0, 0 },
                                            { 0, 40, 30, 30, 0, 0, 0, 0, 0, 0 },
                                            { 0, 30, 40, 30, 0, 0, 0, 0, 0, 0 },
                                            { 0, 30, 30, 40, 0, 0, 0, 0, 0, 0 },
                                            { 0, 20, 40, 40, 0, 0, 0, 0, 0, 0 }
                                        };
    private int[,,] TierRaceJobPoolSize;

    // Constructor
    public CharacterGenerator()
    {
        rngesus = new System.Random();

        //creating pools for race and job limits
        TierRaceJobPoolSize = new int[tiersRaceJobPoolMax.GetLength(0), Enum.GetNames(typeof(Enums.Race)).Length, Enum.GetNames(typeof(Enums.Job)).Length];
        for (int i = 0; i < tiersRaceJobPoolMax.GetLength(0); i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(Enums.Race)).Length; j++)
            {
                for (int k = 0; k < Enum.GetNames(typeof(Enums.Job)).Length; k++)
                {
                    TierRaceJobPoolSize[i, j, k] = tiersRaceJobPoolMax[i, j, k];
                }
            }
        }
    }

    public Piece GenerateCharacter(int currentRarityTier)
    {
        //calculate character rarity
        currentRarityTier = Math.Min(currentRarityTier - 1, rarityUpgradeTiers.GetLength(0) - 1);
        int rarityTotalPool = 0;
        for (int i = 0; i < TierRaceJobPoolSize.GetLength(0); i++)
        {
            rarityTotalPool += rarityUpgradeTiers[currentRarityTier, i];
        }
        int rarityNumber = rngesus.Next(1, rarityTotalPool);
        int characterRarity = 0;
        for (int i = 0; i < TierRaceJobPoolSize.GetLength(0); i++)
        {
            rarityNumber -= rarityUpgradeTiers[currentRarityTier, i];
            if (rarityNumber <= 0)
            {
                characterRarity = i;
                break;
            }
        }

        //calculate race and job
        int totalPool = 0;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++) {
            for (int j = 0; j < Enum.GetNames(typeof(Enums.Job)).Length; j++) {
                totalPool += TierRaceJobPoolSize[characterRarity,i,j];
            }
        }

        //in case all units are bought out from the current rarity tier, repopulate one in each
        if (totalPool < 1)
        {
            for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(Enums.Job)).Length; j++)
                {
                    TierRaceJobPoolSize[characterRarity, i, j] = 1;
                    totalPool++;
                }
            }
        }

        int poolNumber = rngesus.Next(1, totalPool + 1);
        Enums.Race race = 0;
        Enums.Job job = 0;
        bool decided = false;
        for (int i = 0; i < Enum.GetNames(typeof(Enums.Race)).Length; i++) {
            for (int j = 0; j < Enum.GetNames(typeof(Enums.Job)).Length; j++) {
                poolNumber -= TierRaceJobPoolSize[characterRarity,i,j];
                if (poolNumber <= 0)
                {
                    race = (Enums.Race)i;
                    job = (Enums.Job)j;
                    decided = true;
                    break;
                }
            }
            if (decided)
                break;
        }

        return GenerateCharacter(characterRarity, job, race);
    }

    public Piece GenerateCharacter(int characterRarity, Enums.Job job, Enums.Race race)
    {
        TierRaceJobPoolSize[characterRarity, (int)race, (int)job]--;
        int currentHitPoints = defaultHitPoints;
        int currentManaPoints = defaultManaPoints;
        int currentAttackDamage = defaultAttackDamage;
        int currentAttackRange = defaultAttackRange;
        int currentAttackSpeed = defaultAttackSpeed;
        int currentMovementSpeed = defaultMovementSpeed;

        //calculate stats
        switch (job)
        {
            case Enums.Job.Druid:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(druidHitPointMultiplier, characterRarity+1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(druidAttackDamageMultiplier, characterRarity+1));
                currentManaPoints += druidManaPointAdditor*(characterRarity+1);
                break;
            case Enums.Job.Priest:
                currentAttackRange += priestFlatAttackRangeAdditor;
                currentManaPoints += priestManaPointAdditor * (characterRarity + 1);
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(priestHitPointMultiplier, characterRarity + 1));
                break;
            case Enums.Job.Knight:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(knightHitPointMultiplier, characterRarity+1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(knightAttackDamageMultiplier, characterRarity+1));
                break;
            case Enums.Job.Rogue:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(rogueHitPointMultiplier, characterRarity+1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(rogueAttackDamageMultiplier, characterRarity + 1));
                currentMovementSpeed += rogueFlatMovementSpeedAdditor;
                currentAttackSpeed += rogueFlatAttackSpeedAdditor;
                break;
            case Enums.Job.Mage:
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(mageAttackDamageMultiplier, characterRarity+1));
                currentAttackRange += mageFlatAttackRangeAdditor;
                currentManaPoints += mageManaPointAdditor*(characterRarity+1);
                break;
        }
        switch (race)
        {
            case Enums.Race.Elf:
                currentMovementSpeed += elfFlatMovementSpeedAdditor;
                currentAttackSpeed += elfFlatAttackSpeedAdditor;
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(elfAttackDamageMultiplier,characterRarity+1));
                currentManaPoints += elfManaPointAdditor * (characterRarity + 1);
                if (currentAttackRange > 1)
                {
                    currentAttackRange += elfFlatConditionalAttackRangeAdditor;
                }
                break;
            case Enums.Race.Orc:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(orcHitPointMultiplier, characterRarity+1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(orcAttackDamageMultiplier, characterRarity+1));
                currentMovementSpeed += orcFlatMovementSpeedAdditor;
                currentAttackSpeed += orcFlatAttackSpeedAdditor;
                break;
            case Enums.Race.Human:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(humanHitPointMultiplier, characterRarity+1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(humanAttackDamageMultiplier, characterRarity+1));
                currentManaPoints += humanManaPointAdditor*(characterRarity+1);
                break;
            case Enums.Race.Undead:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(undeadHitPointMultiplier, characterRarity+1));
                currentAttackSpeed += undeadFlatAttackSpeedAdditor;
                currentHitPoints += undeadFlatMovementSpeedAdditor;
                break;
        }
        if (race == Enums.Race.Elf && job == Enums.Job.Druid)
        {
            currentAttackRange++;
        }
        if (race == Enums.Race.Human && job == Enums.Job.Priest)
        {
            currentManaPoints = (int)Math.Floor(currentManaPoints * humanPriestManaMultiplier);
        }
        Piece currentPiece = new Piece(NameGenerator.GenerateName(job, race), NameGenerator.GetTitle(race, job), race, job, characterRarity + 1, false,
                                       currentHitPoints, currentManaPoints,
                                       currentAttackDamage, currentAttackRange,
                                       currentAttackSpeed, currentMovementSpeed);

        return currentPiece;
    }

    public void ReturnPiece(Piece piece)
    {
        TierRaceJobPoolSize[piece.GetRarity() - 1, (int)piece.GetRace(), (int)piece.GetClass()]++;
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

    public bool TryUpgradeCharacterMarketTier(ref Piece piece, int currentMarketTier)
    {
        //if new rarity does not exist
        if (piece.GetRarity() > tiersRaceJobPoolMax.GetLength(0))
            return false;
        //if character rarity can be found in the new market tier
        if (rarityUpgradeTiers[currentMarketTier - 1, piece.GetRarity()] > 0)
        {
            //if character is not out of stock in the new rarity tier
            if (TierRaceJobPoolSize[piece.GetRarity(), (int)piece.GetRace(),(int)piece.GetClass()] > 0)
            {
                ReturnPiece(piece);
                Piece tempPiece = GenerateCharacter(piece.GetRarity(), piece.GetClass(), piece.GetRace());
                tempPiece.SetName(piece.GetName());
                piece = tempPiece;
                return true;
            }
        }
        return false;
    }

    public bool TryUpgradeCharacterRoundsSurvived(Piece piece)
    {
        // Don't upgrade the piece if it has not survived enough rounds or is at max rarity.
        if (piece.GetRoundsSurvived() < 2 || piece.GetRarity() >= 4)
        {
            return false;
        }

        // Calculate upgraded Stats
        int currentHitPoints = defaultHitPoints;
        int currentManaPoints = defaultManaPoints;
        int currentAttackDamage = defaultAttackDamage;
        int currentAttackRange = defaultAttackRange;
        int currentAttackSpeed = defaultAttackSpeed;
        int currentMovementSpeed = defaultMovementSpeed;
        int characterRarity = piece.GetRarity() + 1;

        switch (piece.GetClass())
        {
            case Enums.Job.Druid:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(druidHitPointMultiplier, characterRarity + 1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(druidAttackDamageMultiplier, characterRarity + 1));
                currentManaPoints += druidManaPointAdditor * (characterRarity + 1);
                break;
            case Enums.Job.Priest:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(priestHitPointMultiplier, characterRarity + 1));
                currentAttackRange += priestFlatAttackRangeAdditor;
                currentManaPoints += priestManaPointAdditor * (characterRarity + 1);
                break;
            case Enums.Job.Knight:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(knightHitPointMultiplier, characterRarity + 1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(knightAttackDamageMultiplier, characterRarity + 1));
                break;
            case Enums.Job.Rogue:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(rogueHitPointMultiplier, characterRarity + 1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(rogueAttackDamageMultiplier, characterRarity + 1));
                currentMovementSpeed += rogueFlatMovementSpeedAdditor;
                currentAttackSpeed += rogueFlatAttackSpeedAdditor;
                break;
            case Enums.Job.Mage:
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(mageAttackDamageMultiplier, characterRarity + 1));
                currentAttackRange += mageFlatAttackRangeAdditor;
                currentManaPoints += mageManaPointAdditor * (characterRarity + 1);
                break;
        }

        switch (piece.GetRace())
        {
            case Enums.Race.Elf:
                currentMovementSpeed += elfFlatMovementSpeedAdditor;
                currentAttackSpeed += elfFlatAttackSpeedAdditor;
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(elfAttackDamageMultiplier, characterRarity + 1));
                currentManaPoints += elfManaPointAdditor * (characterRarity + 1);
                if (currentAttackRange > 1)
                {
                    currentAttackRange += elfFlatConditionalAttackRangeAdditor;
                }
                break;
            case Enums.Race.Orc:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(orcHitPointMultiplier, characterRarity + 1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(orcAttackDamageMultiplier, characterRarity + 1));
                currentMovementSpeed += orcFlatMovementSpeedAdditor;
                currentAttackSpeed += orcFlatAttackSpeedAdditor;
                break;
            case Enums.Race.Human:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(humanHitPointMultiplier, characterRarity + 1));
                currentAttackDamage = (int)Math.Floor(currentAttackDamage * Math.Pow(humanAttackDamageMultiplier, characterRarity + 1));
                currentManaPoints += humanManaPointAdditor * (characterRarity + 1);
                break;
            case Enums.Race.Undead:
                currentHitPoints = (int)Math.Floor(currentHitPoints * Math.Pow(undeadHitPointMultiplier, characterRarity + 1));
                currentAttackSpeed += undeadFlatAttackSpeedAdditor;
                currentHitPoints += undeadFlatMovementSpeedAdditor;
                break;
        }

        // Set the upgraded stats to the piece.
        piece.SetDefaultMaximumHitPoints(currentHitPoints);
        piece.SetMaximumHitPoints(currentHitPoints);
        piece.SetMaximumManaPoints(currentManaPoints);
        piece.SetCurrentManaPoints(0);
        piece.SetDefaultAttackDamage(currentAttackDamage);
        piece.SetAttackDamage(currentAttackDamage);
        piece.SetDefaultAttackRange(currentAttackRange);
        piece.SetAttackRange(currentAttackRange);
        piece.SetDefaultAttackSpeed(currentAttackSpeed);
        piece.SetAttackSpeed(currentAttackSpeed);
        piece.SetDefaultMovementSpeed(currentMovementSpeed);
        piece.SetMovementSpeed(currentMovementSpeed);

        Debug.Log(piece.GetName() + " has upgraded from Rarity " + piece.GetRarity() + " to " + characterRarity + "!");
        piece.SetRarity(characterRarity);
        piece.SetRoundsSurvived(0);
        return true;
    }
}
