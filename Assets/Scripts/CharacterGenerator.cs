using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterGenerator
{
    private string name;
    private System.Random rngesus;

    public readonly int defaultHitPoints = GameLogicManager.Inst.Data.Gen.DefaultHitPoints;
    public readonly int defaultManaPoints = GameLogicManager.Inst.Data.Gen.DefaultManaPoints;
    public readonly int defaultAttackDamage = GameLogicManager.Inst.Data.Gen.DefaultAttackDamage;
    public readonly int defaultAttackRange = GameLogicManager.Inst.Data.Gen.DefaultAttackRange;
    public readonly int defaultAttackSpeed = GameLogicManager.Inst.Data.Gen.DefaultAttackSpeed;
    public readonly int minimumAttackSpeed = GameLogicManager.Inst.Data.Gen.MinAttackSpeed;
    public readonly int maximumAttackSpeed = GameLogicManager.Inst.Data.Gen.MaxAttackSpeed;
    public readonly int defaultMovementSpeed = GameLogicManager.Inst.Data.Gen.DefaulMovementSpeed;
    public readonly int minimumMovementSpeed = GameLogicManager.Inst.Data.Gen.MinMovementSpeed;
    public readonly int maximumMovementSpeed = GameLogicManager.Inst.Data.Gen.MaxMovementSpeed;
    public readonly double hitPointMultiplier = GameLogicManager.Inst.Data.Gen.HitPointMultiplier;
    public readonly double attackDamageMultiplier = GameLogicManager.Inst.Data.Gen.AttackDamageMultiplier;

    public readonly double humanPriestManaMultiplier = GameLogicManager.Inst.Data.Gen.HumanPriestManaMultiplier;
    public readonly double elfDruidManaMultiplier = GameLogicManager.Inst.Data.Gen.ElfDruidManaMultiplier;

    //human stat changes
    public readonly double humanHitPointMultiplier = GameLogicManager.Inst.Data.Gen.HumanHitPointMultiplier;
    public readonly double humanAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.HumanAttackDamageMultiplier;
    public readonly int humanManaPointAdditor = GameLogicManager.Inst.Data.Gen.HumanManaPointAdditor;

    //orc stat changes
    public readonly int orcFlatMovementSpeedAdditor = GameLogicManager.Inst.Data.Gen.OrcFlatMovementSpeedAdditor;
    public readonly double orcHitPointMultiplier = GameLogicManager.Inst.Data.Gen.OrcHitPointMultipler;
    public readonly double orcAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.OrcAttackDamageMultiplier;
    public readonly int orcFlatAttackSpeedAdditor = GameLogicManager.Inst.Data.Gen.OrcFlatAttackSpeedAdditor;

    //elf stat changes
    public readonly int elfFlatMovementSpeedAdditor = GameLogicManager.Inst.Data.Gen.ElfFlatMovementSpeedAdditor;
    public readonly int elfFlatAttackSpeedAdditor = GameLogicManager.Inst.Data.Gen.ElfFlatAttackSpeedAdditor;
    public readonly int elfFlatConditionalAttackRangeAdditor = GameLogicManager.Inst.Data.Gen.ElfFlatConditionalAttackRangeAdditor;
    public readonly double elfAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.ElfAttackDamageMultiplier;
    public readonly int elfManaPointAdditor = GameLogicManager.Inst.Data.Gen.ElfManaPointAdditor;

    //undead stat changes
    public readonly int undeadFlatMovementSpeedAdditor = GameLogicManager.Inst.Data.Gen.UndeadFlatMovementSpeedAdditor;
    public readonly int undeadFlatAttackSpeedAdditor = GameLogicManager.Inst.Data.Gen.UndeadFlatAttackSpeedAdditor;
    public readonly double undeadHitPointMultiplier = GameLogicManager.Inst.Data.Gen.UndeadHitPointMultiplier;

    //mage stat changes
    public readonly int mageFlatAttackRangeAdditor = GameLogicManager.Inst.Data.Gen.MageFlatAttackRangeAdditor;
    public readonly int mageManaPointAdditor = GameLogicManager.Inst.Data.Gen.MageManaPointAdditor;
    public readonly double mageAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.MageAttackDamageMultiplier;

    //rogue stat changes
    public readonly int rogueFlatMovementSpeedAdditor = GameLogicManager.Inst.Data.Gen.RogueFlatMovementSpeedAdditor;
    public readonly int rogueFlatAttackSpeedAdditor = GameLogicManager.Inst.Data.Gen.RogueFlatAttackSpeedAdditor;
    public readonly double rogueHitPointMultiplier = GameLogicManager.Inst.Data.Gen.RogueHitPointMultiplier;
    public readonly double rogueAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.RogueAttackDamageMultiplier;

    //druid stat changes
    public readonly double druidHitPointMultiplier = GameLogicManager.Inst.Data.Gen.DruidHitPointMultiplier;
    public readonly double druidAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.DruidAttackDamageMultiplier;
    public readonly int druidManaPointAdditor = GameLogicManager.Inst.Data.Gen.DruidManaPointAdditor;

    //knight stat changes
    public readonly double knightHitPointMultiplier = GameLogicManager.Inst.Data.Gen.KnightHitPointMultiplier;
    public readonly double knightAttackDamageMultiplier = GameLogicManager.Inst.Data.Gen.KnightAttackDamageMultiplier;

    //priest stat changes
    public readonly int priestFlatAttackRangeAdditor = GameLogicManager.Inst.Data.Gen.PriestFlatAttackRangeAdditor;
    public readonly int priestManaPointAdditor = GameLogicManager.Inst.Data.Gen.PriestManaPointAdditor;
    public readonly double priestHitPointMultiplier = GameLogicManager.Inst.Data.Gen.PriestHitPointMultiplier;

    public readonly int[,,] tiersRaceJobPoolMax = {
        {
            { 0, 0, 5, 0, 5 },
            { 0, 5, 0, 5, 0 },
            { 5, 5, 0, 0, 5 },
            { 5, 0, 5, 0, 0 }
        },
        {
            { 0, 3, 0, 3, 0 },
            { 3, 0, 3, 0, 0 },
            { 0, 0, 0, 3, 0 },
            { 0, 3, 0, 0, 3 }
        },
        {
            { 2, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 2 },
            { 0, 0, 2, 0, 0 },
            { 0, 0, 0, 2, 0 }
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
                                            { 70, 30, 0, 0, 0, 0, 0, 0, 0, 0 },
                                            { 50, 45, 5, 0, 0, 0, 0, 0, 0, 0 },
                                            { 35, 125, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 },
                                            { 5, 55, 40, 0, 0, 0, 0, 0, 0, 0 }
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
                    TierRaceJobPoolSize[characterRarity, i, j] = tiersRaceJobPoolMax[characterRarity, i, j];
                    if (tiersRaceJobPoolMax[characterRarity, i, j] > 0)
                        totalPool += tiersRaceJobPoolMax[characterRarity, i, j];
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

        Debug.Log(rogueFlatMovementSpeedAdditor);

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
            currentManaPoints = (int)Math.Floor(currentManaPoints * elfDruidManaMultiplier);
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
