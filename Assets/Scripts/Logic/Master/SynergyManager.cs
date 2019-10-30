using UnityEngine;
using System;
using System.Linq;

public class SynergyManager : MonoBehaviour
{
    private double undeadLifestealPercentage = 0.35;
    private int elfRangeModifier = 3;
    private double knightRecoilPercentage = 0.35;
    private int rogueHealthDivider = 4;
    private int priestStatMultiplier = 3;
    private double mageStartingManaPercentage = 2.0 / 3.0;
    private double orcHealthMultiplier = 1.5;

    private int[] jobSynergyCount = new int[Enum.GetNames(typeof(Enums.Job)).Length];
    private int[] raceSynergyCount = new int[Enum.GetNames(typeof(Enums.Race)).Length];
    public static int[] jobSynergyRequirement = new int[] { 3, 3, 3, 3, 3 };
    public static int[] raceSynergyRequirement = new int[] { 3, 3, 3, 3 };

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
            if (jobSynergyCount[i] >= 1 || playerInv.HasSynergy((Enums.Job)i))
            {
                ApplyJobSynergy(board, i);
            }
        }
        for (int i = 0; i < raceSynergyCount.Length; i++)
        {
            if (raceSynergyCount[i] >= 1 || playerInv.HasSynergy((Enums.Race)i))
            {
                ApplyRaceSynergy(board, i);
            }
        }
    }

    private void ApplyJobSynergy(Board board, int i)
    {
        var friendlyPieces = board.GetActiveFriendliesOnBoard();
        var randomFriendly = rngesus.Next(0, friendlyPieces.Count);
        var enemyPieces = board.GetActiveEnemiesOnBoard();
        var randomEnemy = rngesus.Next(0, enemyPieces.Count);
        switch (i)
        {
            case (int)Enums.Job.Druid://druids become equally strong
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Druid)
                    {
                        friendlyPieces[target].SetAttackDamage(friendlyPieces.FindAll(p => p.GetClass() == Enums.Job.Druid).Max(p => p.GetAttackDamage()));
                        friendlyPieces[target].SetMaximumHitPoints(friendlyPieces.FindAll(p => p.GetClass() == Enums.Job.Druid).Max(p => p.GetMaximumHitPoints()));
                        friendlyPieces[target].SetCurrentHitPoints(friendlyPieces.FindAll(p => p.GetClass() == Enums.Job.Druid).Max(p => p.GetCurrentHitPoints()));
                        friendlyPieces[target].SetAttackRange(friendlyPieces.FindAll(p => p.GetClass() == Enums.Job.Druid).Max(p => p.GetAttackRange()));
                        friendlyPieces[target].SetMovementSpeed(friendlyPieces.FindAll(p => p.GetClass() == Enums.Job.Druid).Max(p => p.GetMovementSpeed()));
                    }
                }
                break;
            case (int)Enums.Job.Knight://knights do recoil damage
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Knight)
                    {
                        friendlyPieces[target].SetRecoilPercentage(friendlyPieces[target].GetRecoilPercentage() + knightRecoilPercentage);
                    }
                }
                break;
            case (int)Enums.Job.Mage://mages start with additional mana
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetClass() == Enums.Job.Mage)
                    {
                        friendlyPieces[target].SetCurrentManaPoints((int)Math.Floor(friendlyPieces[target].GetMaximumManaPoints() * mageStartingManaPercentage));
                    }
                }
                break;
            case (int)Enums.Job.Priest://priests heaviliy buffs a character
                friendlyPieces[randomFriendly].SetAttackDamage(friendlyPieces[randomFriendly].GetAttackDamage() * priestStatMultiplier);
                friendlyPieces[randomFriendly].SetMaximumHitPoints(friendlyPieces[randomFriendly].GetMaximumHitPoints() * priestStatMultiplier);
                friendlyPieces[randomFriendly].SetCurrentHitPoints(friendlyPieces[randomFriendly].GetMaximumHitPoints());
                break;
            case (int)Enums.Job.Rogue:
                enemyPieces[randomEnemy].SetCurrentHitPoints(enemyPieces[randomEnemy].GetMaximumHitPoints() / rogueHealthDivider);
                break;
            default:
                Debug.Log("Error, unknown job synergy found: " + ((Enums.Job)i).ToString());
                break;
        }
    }

    private void ApplyRaceSynergy(Board board, int i)
    {
        var friendlyPieces = board.GetActiveFriendliesOnBoard();
        var randomFriendly = rngesus.Next(0, friendlyPieces.Count);
        var enemyPieces = board.GetActiveEnemiesOnBoard();
        var randomEnemy = rngesus.Next(0, enemyPieces.Count);
        switch (i)
        {
            case (int)Enums.Race.Elf://elves gain extra range
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Elf)
                    {
                        friendlyPieces[target].SetAttackRange(friendlyPieces[target].GetAttackRange() + elfRangeModifier);
                    }
                }
                break;
            case (int)Enums.Race.Human://humans activate a random race synergy
                int randomRace = rngesus.Next(0, Enum.GetNames(typeof(Enums.Race)).Length);
                ApplyRaceSynergyToHuman(board, randomRace);
                break;
            case (int)Enums.Race.Orc://orcs gain health
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Orc)
                    {
                        friendlyPieces[target].SetMaximumHitPoints((int)Math.Floor(friendlyPieces[target].GetMaximumHitPoints() * orcHealthMultiplier));
                        friendlyPieces[target].SetCurrentHitPoints(friendlyPieces[target].GetMaximumHitPoints());
                    }
                }
                break;
            case (int)Enums.Race.Undead://undead gain lifesteal
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Undead)
                    {
                        friendlyPieces[target].SetLifestealPercentage(friendlyPieces[target].GetLifestealPercentage() + undeadLifestealPercentage);
                    }
                }
                break;
            default:
                Debug.Log("Error, unknown race synergy found: " + ((Enums.Race)i).ToString());
                break;
        }
    }

    private void ApplyRaceSynergyToHuman(Board board, int i)
    {
        var friendlyPieces = board.GetActiveFriendliesOnBoard();
        var randomFriendly = rngesus.Next(0, friendlyPieces.Count);
        var enemyPieces = board.GetActiveEnemiesOnBoard();
        var randomEnemy = rngesus.Next(0, enemyPieces.Count);
        switch (i)
        {
            case (int)Enums.Race.Elf://elves gain extra range
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Human)
                    {
                        friendlyPieces[target].SetAttackRange(friendlyPieces[target].GetAttackRange() + elfRangeModifier);
                    }
                }
                break;
            case (int)Enums.Race.Human://humans activate a random race synergy
                int randomRace = rngesus.Next(0, Enum.GetNames(typeof(Enums.Race)).Length);
                ApplyRaceSynergyToHuman(board, randomRace);
                break;
            case (int)Enums.Race.Orc://orcs gain health
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Human)
                    {
                        friendlyPieces[target].SetMaximumHitPoints((int)Math.Floor(friendlyPieces[target].GetMaximumHitPoints() * orcHealthMultiplier));
                        friendlyPieces[target].SetCurrentHitPoints(friendlyPieces[target].GetMaximumHitPoints());
                    }
                }
                break;
            case (int)Enums.Race.Undead://undead gain lifesteal
                for (int target = 0; target < friendlyPieces.Count; target++)
                {
                    if (friendlyPieces[target].GetRace() == Enums.Race.Human)
                    {
                        friendlyPieces[target].SetLifestealPercentage(friendlyPieces[target].GetLifestealPercentage() + undeadLifestealPercentage);
                    }
                }
                break;
            default:
                Debug.Log("Error, unknown race synergy found: " + ((Enums.Race)i).ToString());
                break;
        }
    }


}
