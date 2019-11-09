using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private int[] incomesGenerated = { 0, 0, 0 };
    private int passiveGoldOnEvenRounds = 1;

    public void GenerateIncome(int round)
    {
        round -= 1;
        // If I'm master client
        for (int i = 0; i < 3; ++i)
        {
            int goldToGive = round;
            if (inventoryManager.synergyManager.HasSynergy(Enums.Race.Human))
            {
                if (inventoryManager.synergyManager.HasBetterSynergy(Enums.Race.Human))
                {
                    goldToGive += inventoryManager.synergyManager.humanGoldAmount;
                }
                else
                {
                    goldToGive += inventoryManager.synergyManager.humanWeakerGoldAmount;
                }
            }
            goldToGive += incomesGenerated[i];
            if (round%2 == 0)
            {
                goldToGive += passiveGoldOnEvenRounds;
            }
            inventoryManager.AddGold((Player)i, goldToGive);
        }
        Debug.Log("Players have earned { " + incomesGenerated[0] + ", " + incomesGenerated[1] + ", " + +incomesGenerated[2] + " }");
    }

    public int GetIncome(int round, int playerIndex)
    {
        if (round == 1)
        {
            return 4;
        }

        round -= 1;
        int goldToGive = round;
        if (inventoryManager.synergyManager.HasSynergy(Enums.Race.Human))
        {
            if (inventoryManager.synergyManager.HasBetterSynergy(Enums.Race.Human))
            {
                goldToGive += inventoryManager.synergyManager.humanGoldAmount;
            }
            else
            {
                goldToGive += inventoryManager.synergyManager.humanWeakerGoldAmount;
            }
        }
        goldToGive += incomesGenerated[playerIndex];
        if (round % 2 == 0)
        {
            goldToGive += passiveGoldOnEvenRounds;
        }
        return goldToGive;
    }

    public int GetIncomeFromRound(int round, int playerIndex)
    {
        if (round == 1)
        {
            return 4;
        }

        round -= 1;
        int goldToGive = round;
        if (round % 2 == 0)
        {
            goldToGive += passiveGoldOnEvenRounds;
        }
        return goldToGive;
    }

    public int GetIncomeFromSynergy(int round, int playerIndex)
    {
        int goldToGive = 0;
        if (inventoryManager.synergyManager.HasSynergy(Enums.Race.Human))
        {
            if (inventoryManager.synergyManager.HasBetterSynergy(Enums.Race.Human))
            {
                goldToGive += inventoryManager.synergyManager.humanGoldAmount;
            }
            else
            {
                goldToGive += inventoryManager.synergyManager.humanWeakerGoldAmount;
            }
        }
        return goldToGive;
    }

    public int GetIncomeFromVictory(int round, int playerIndex)
    {
        int goldToGive = 0;
        goldToGive += incomesGenerated[playerIndex];
        return goldToGive;
    }

    public void SetIncomeGeneratedByPlayer(Player player, int incomeGenerated)
    {
        incomesGenerated[(int) player] = incomeGenerated;
    }
}
