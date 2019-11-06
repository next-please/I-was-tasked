using UnityEngine;
using System;

public class SynergyManager : MonoBehaviour
{
    private double mageStartingManaPercentage = GameLogicManager.Inst.Data.Synergy.MageStartingManaPercentage;
    public int humanWeakerGoldAmount = GameLogicManager.Inst.Data.Synergy.HumanGoldAmount1;
    public int humanGoldAmount = GameLogicManager.Inst.Data.Synergy.HumanGoldAmount2;
    private double rogueDamageMultiplier = GameLogicManager.Inst.Data.Synergy.RogueDamageMultiplier;
    private double rogueHitPointMultiplier = GameLogicManager.Inst.Data.Synergy.RogueHitPointMultiplier;
    private int undeadBonusHealth = GameLogicManager.Inst.Data.Synergy.UndeadBonusHealth;
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
        GameLogicManager.Inst.Data.Synergy.DruidRequirement1,
        GameLogicManager.Inst.Data.Synergy.KnightRequirement1,
        GameLogicManager.Inst.Data.Synergy.MageRequirement1,
        GameLogicManager.Inst.Data.Synergy.PriestRequirement1,
        GameLogicManager.Inst.Data.Synergy.RogueRequirement1,
    };

    public static int[] jobSynergyHigherRequirement = new int[]
    {
        GameLogicManager.Inst.Data.Synergy.DruidRequirement2,
        GameLogicManager.Inst.Data.Synergy.KnightRequirement2,
        GameLogicManager.Inst.Data.Synergy.MageRequirement2,
        GameLogicManager.Inst.Data.Synergy.PriestRequirement2,
        GameLogicManager.Inst.Data.Synergy.RogueRequirement2,
    };

    public static int[] raceSynergyRequirement = new int[]
    {
        GameLogicManager.Inst.Data.Synergy.HumanRequirement1,
        GameLogicManager.Inst.Data.Synergy.ElfRequirement1,
        GameLogicManager.Inst.Data.Synergy.OrcRequirement1,
        GameLogicManager.Inst.Data.Synergy.UndeadRequirement1
    };

    public static int[] raceSynergyHigherRequirement = new int[]
    {
        GameLogicManager.Inst.Data.Synergy.HumanRequirement2,
        GameLogicManager.Inst.Data.Synergy.ElfRequirement2,
        GameLogicManager.Inst.Data.Synergy.OrcRequirement2,
        GameLogicManager.Inst.Data.Synergy.UndeadRequirement2
    };

    public InventoryManager inventoryManager;
    public BoardManager boardManager;
    public System.Random rngesus;

    private static SynergyManager instance;

    void Awake()
    {
        instance = this;
    }

    public static SynergyManager GetInstance()
    {
        return instance;
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
        if (job == Enums.Job.Rogue)
        {
            return jobSynergyCount[(int)job] >= jobSynergyHigherRequirement[(int)job] || jobSynergyCount[(int)job] == jobSynergyRequirement[(int)job];
        }
        return jobSynergyCount[(int)job] >= jobSynergyRequirement[(int)job];
    }

    public bool HasSynergy(Enums.Race race)
    {
        return raceSynergyCount[(int)race] >= raceSynergyRequirement[(int)race];
    }

    public bool HasBetterSynergy(Enums.Race race)
    {
        return raceSynergyCount[(int)race] >= raceSynergyHigherRequirement[(int)race];
    }

    public bool HasBetterSynergy(Enums.Job job)
    {
        return jobSynergyCount[(int)job] >= jobSynergyHigherRequirement[(int)job];
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
            if ((Enums.Job.Rogue == (Enums.Job)i))
            {
                if (jobSynergyCount[i] >= jobSynergyHigherRequirement[i] || jobSynergyCount[i] == jobSynergyRequirement[i])
                {
                    ApplyJobSynergy(board, i, player);
                }
            }
            else
            {
                if (jobSynergyCount[i] >= jobSynergyRequirement[i])
                {
                    ApplyJobSynergy(board, i, player);
                }

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
                if (jobSynergyCount[(int)Enums.Job.Druid] >= jobSynergyHigherRequirement[(int)Enums.Job.Druid])
                {
                    for (int target = 0; target < enemyPieces.Count; target++)
                    {
                        enemyPieces[target].SetAttackSpeed(enemyPieces[target].GetAttackSpeed() - 1);
                        enemyPieces[target].SetMovementSpeed(enemyPieces[target].GetMovementSpeed() - 1);
                    }
                }

                break;
            case (int)Enums.Job.Knight://knights taunt enemies
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Knight)
                    {
                        friendlyPieces[target].taunting = true;

                        if (jobSynergyCount[(int)Enums.Job.Knight] >= jobSynergyHigherRequirement[(int)Enums.Job.Knight])
                        {
                            friendlyPieces[target].invulnerable = true;
                            board.AddInteractionToProcess(new UnstoppableSynergyEffect(friendlyPieces[target], board));
                        }
                    }
                }
                break;
            case (int)Enums.Job.Mage://mages start with additional mana and doublecast
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Mage)
                    {
                        friendlyPieces[target].SetCurrentManaPoints((int)Math.Floor(friendlyPieces[target].GetMaximumManaPoints() * mageStartingManaPercentage));

                        if (jobSynergyCount[(int)Enums.Job.Mage] >= jobSynergyHigherRequirement[(int)Enums.Job.Mage])
                        {
                            friendlyPieces[target].multicast = true;
                        }
                    }
                }
                break;
            case (int)Enums.Job.Priest://priests cast divine judgement on death
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Priest)
                    {
                        if (jobSynergyCount[(int)Enums.Job.Priest] >= jobSynergyHigherRequirement[(int)Enums.Job.Priest])
                        {
                            board.AddInteractionToProcess(new RetributionSynergyEffect(friendlyPieces[target], board, priestRetributionRadius, priestRetributionDamage, priestRetributionHealing));
                        }
                        else
                        {
                            board.AddInteractionToProcess(new RetributionSynergyEffect(friendlyPieces[target], board, priestRetributionRadius/2, priestRetributionDamage/2, priestRetributionHealing/2));
                        }
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
                    if (friendlyPieces[target].GetRace() == Enums.Race.Elf || raceSynergyCount[(int)Enums.Race.Elf] >= raceSynergyHigherRequirement[(int)Enums.Race.Elf])
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
                        if (raceSynergyCount[(int)Enums.Race.Orc] >= raceSynergyHigherRequirement[(int)Enums.Race.Orc])
                        {
                            board.AddInteractionToProcess(
                                new RampageSynergyEffect(
                                    friendlyPieces[target],
                                    board,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageAttackSpeed,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageArmourPercentage,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageHealthThreshold2
                                )
                            );
                        }
                        else
                        {
                            board.AddInteractionToProcess(
                                new RampageSynergyEffect(
                                    friendlyPieces[target],
                                    board,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageAttackSpeed,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageArmourPercentage,
                                    GameLogicManager.Inst.Data.Synergy.OrcRampageHealthThreshold1
                                )
                            );
                        }
                    }
                }
                break;
            case (int)Enums.Race.Undead://undead are immortal. then they die
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Undead)
                    {
                        friendlyPieces[target].SetMaximumHitPoints(friendlyPieces[target].GetMaximumHitPoints() + (int)((double)undeadBonusHealth / raceSynergyCount[(int)Enums.Race.Undead]));
                        friendlyPieces[target].SetCurrentHitPoints(friendlyPieces[target].GetCurrentHitPoints() + (int)((double)undeadBonusHealth / raceSynergyCount[(int)Enums.Race.Undead]));

                        if (raceSynergyCount[(int)Enums.Race.Undead] >= raceSynergyHigherRequirement[(int)Enums.Race.Undead])
                        {
                            friendlyPieces[target].invulnerable = true;
                            friendlyPieces[target].SetMaximumHitPoints(undeadTicksToDie);
                            friendlyPieces[target].SetCurrentHitPoints(undeadTicksToDie);
                            board.AddInteractionToProcess(new DecayingSynergyEffect(friendlyPieces[target]));
                        }
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
    public bool Applied;
}

public class JobSynergyAppliedEvent : GameEvent
{
    public Enums.Job Job;
    public bool Applied;
}
