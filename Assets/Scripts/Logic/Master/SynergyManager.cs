﻿using UnityEngine;
using System;
using System.Linq;

public class SynergyManager : MonoBehaviour
{
    private double undeadLifestealPercentage = 0.35;
    private int elfRangeModifier = 3;
    private double knightRecoilPercentage = 0.35;
    private int rogueHealthDivider = 4;
    private int priestStatMultiplier = 3;
    private double orcHealthMultiplier = 1.5;

    private double mageStartingManaPercentage = GameLogicManager.Inst.Data.Synergy.MageStartingManaPercentage;
    public int humanGoldAmount = GameLogicManager.Inst.Data.Synergy.HumanGoldAmount;
    private double rogueDamageMultiplier = GameLogicManager.Inst.Data.Synergy.RogueDamageMultiplier;
    private double rogueHitPointMultiplier = GameLogicManager.Inst.Data.Synergy.RogueHitPointMultiplier;
    private int undeadTicksToDie = GameLogicManager.Inst.Data.Synergy.UndeadTicksToDie;
    private int priestRetributionRadius = GameLogicManager.Inst.Data.Synergy.PriestRetributionRadius;
    private int priestRetributionDamage = GameLogicManager.Inst.Data.Synergy.PriestRetributionDamage;
    private int priestRetributionHealing = GameLogicManager.Inst.Data.Synergy.PriestRetributionHealing;
    private int elfGuidingSpiritAttackDamage = GameLogicManager.Inst.Data.Synergy.ElfGuidingSpiritAttackDamage;
    private int elfGuidingSpiritAttackSpeed = GameLogicManager.Inst.Data.Synergy.ElfGuidingSpiritAttackSpeed;

    private int[] jobSynergyCount = new int[Enum.GetNames(typeof(Enums.Job)).Length];
    private int[] raceSynergyCount = new int[Enum.GetNames(typeof(Enums.Race)).Length];
    public static int[] jobSynergyRequirement = new int[]
    {
        GameLogicManager.Inst.Data.Synergy.DruidRequirement,
        GameLogicManager.Inst.Data.Synergy.KnightRequirement,
        GameLogicManager.Inst.Data.Synergy.MageRequirement,
        GameLogicManager.Inst.Data.Synergy.PriestRequirement,
        GameLogicManager.Inst.Data.Synergy.RogueRequirement
    };
    public static int[] raceSynergyRequirement = new int[]
    {
        GameLogicManager.Inst.Data.Synergy.HumanRequirement,
        GameLogicManager.Inst.Data.Synergy.ElfRequirement,
        GameLogicManager.Inst.Data.Synergy.OrcRequirement,
        GameLogicManager.Inst.Data.Synergy.UndeadRequirement
    };

    public InventoryManager inventoryManager;
    public BoardManager boardManager;
    public System.Random rngesus;

    public SynergyManager()
    {
    }

    public void SetSeed(int seed)
    {
        rngesus = new System.Random(seed);
    }

    public void IncreaseSynergyCount(Enums.Job job)
    {
        jobSynergyCount[(int)job]++;
    }

    public void IncreaseSynergyCount(Enums.Race race)
    {
        raceSynergyCount[(int)race]++;
    }

    public void DecreaseSynergyCount(Enums.Job job)
    {
        jobSynergyCount[(int)job]--;
    }

    public void DecreaseSynergyCount(Enums.Race race)
    {
        raceSynergyCount[(int)race]--;
    }

    public void Reset()
    {
        for (int i = 0; i < jobSynergyCount.Length; i++)
        {
            jobSynergyCount[i] = 0;
        }
        for (int i = 0; i < raceSynergyCount.Length; i++)
        {
            raceSynergyCount[i] = 0;
        }
    }

    public bool HasSynergy(Enums.Job job)
    {
        return jobSynergyCount[(int)job] >= jobSynergyRequirement[(int)job];
    }

    public bool HasSynergy(Enums.Race race)
    {
        return raceSynergyCount[(int)race] >= raceSynergyRequirement[(int)race];
    }

    public void ApplySynergiesToArmies(int NumPlayers)
    {
        for (int i = 0; i < NumPlayers; i++)
        {
            ApplySynergiesToArmy(i);
        }
    }

    private void ApplySynergiesToArmy(int index)
    {
        Player player = (Player)index;
        var playerInv = inventoryManager.GetPlayerInventory(player);
        var board = boardManager.GetBoard(player);
        for (int i = 0; i < jobSynergyCount.Length; i++)
        {
            if (jobSynergyCount[i] >= jobSynergyRequirement[i])
            {
                ApplyJobSynergy(board, i, player);
            }
        }
        for (int i = 0; i < raceSynergyCount.Length; i++)
        {
            if (raceSynergyCount[i] >= raceSynergyRequirement[i])
            {
                ApplyRaceSynergy(board, i, player);
            }
        }
    }

    private void ApplyJobSynergy(Board board, int i, Player player)
    {
        var friendlyPieces = board.GetActiveFriendliesOnBoard();
        var randomFriendly = rngesus.Next(0, friendlyPieces.Count);
        var enemyPieces = board.GetActiveEnemiesOnBoard();
        var randomEnemy = rngesus.Next(0, enemyPieces.Count);
        switch (i)
        {
            case (int)Enums.Job.Druid://druids become immobile and gain range
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Druid)
                    {
                        friendlyPieces[target].SetAttackRange(99);
                    }
                }
                break;
            case (int)Enums.Job.Knight://knights taunt enemies
                EventManager.Instance.Raise(new JobSynergyAppliedEvent{ Job = Enums.Job.Knight });
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Knight)
                    {
                        friendlyPieces[target].taunting = true;
                    }
                }
                break;
            case (int)Enums.Job.Mage://mages start with additional mana and doublecast
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Mage)
                    {
                        friendlyPieces[target].SetCurrentManaPoints((int)Math.Floor(friendlyPieces[target].GetMaximumManaPoints() * mageStartingManaPercentage));
                        friendlyPieces[target].multicast = true;
                    }
                }
                break;
            case (int)Enums.Job.Priest://priests cast divine judgement on death
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Priest)
                    {
                        board.AddInteractionToProcess(new RetributionSynergyEffect(friendlyPieces[target], board, priestRetributionRadius, priestRetributionDamage, priestRetributionHealing));
                    }
                }
                break;
            case (int)Enums.Job.Rogue://rogues gain massive damage but lose massive health
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Rogue)
                    {
                        friendlyPieces[target].SetAttackDamage((int)Math.Floor(friendlyPieces[target].GetAttackDamage() * rogueDamageMultiplier));
                        friendlyPieces[target].SetCurrentHitPoints((int)Math.Floor(friendlyPieces[target].GetCurrentHitPoints() * rogueHitPointMultiplier));
                    }
                }
                break;
            default:
                Debug.Log("Error, unknown job synergy found: " + ((Enums.Job)i).ToString());
                break;
        }
    }

    private void ApplyRaceSynergy(Board board, int i, Player player)
    {
        var friendlyPieces = board.GetActiveFriendliesOnBoard();
        var randomFriendly = rngesus.Next(0, friendlyPieces.Count);
        var enemyPieces = board.GetActiveEnemiesOnBoard();
        var randomEnemy = rngesus.Next(0, enemyPieces.Count);
        switch (i)
        {
            case (int)Enums.Race.Elf://elves guide a friendly unit in death
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Elf)
                    {
                        GuidingSpiritSynergyEffect skill = new GuidingSpiritSynergyEffect(friendlyPieces[target], board, elfGuidingSpiritAttackDamage, elfGuidingSpiritAttackSpeed);
                        board.AddInteractionToProcess(skill);
                        friendlyPieces[target].interactions.Add(skill);
                    }
                }
                break;
            case (int)Enums.Race.Human://humans get money
                break;
            case (int)Enums.Race.Orc://orcs rule the world! and they're angry!
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Orc)
                    {
                        board.AddInteractionToProcess(new RampageSynergyEffect(friendlyPieces[target], board));
                    }
                }
                break;
            case (int)Enums.Race.Undead://undead are immortal. then they die
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Undead)
                    {
                        friendlyPieces[target].invulnerable = true;
                        friendlyPieces[target].SetMaximumHitPoints(undeadTicksToDie);
                        friendlyPieces[target].SetCurrentHitPoints(undeadTicksToDie);
                        board.AddInteractionToProcess(new DecayingSynergyEffect(friendlyPieces[target]));
                    }
                }
                break;
            default:
                Debug.Log("Error, unknown race synergy found: " + ((Enums.Race)i).ToString());
                break;
        }
    }

}

public class RaceSynergyAppliedEvent : GameEvent
{
    public Enums.Race Race;
}

public class JobSynergyAppliedEvent : GameEvent
{
    public Enums.Job Job;
}
