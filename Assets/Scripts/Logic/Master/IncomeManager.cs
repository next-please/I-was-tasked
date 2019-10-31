using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private int[] incomesGenerated = { 0, 0, 0 };
    private int passiveGoldOnEvenRounds = 1;
    private int round = 0; // Players earn end-round goal equal to the round number.

    public void GenerateIncome()
    {
        // If I'm master client
        for (int i = 0; i < 3; ++i)
        {
            int goldToGive = round;
            if (inventoryManager.synergyManager.HasSynergy(Enums.Race.Human))
            {
                goldToGive += inventoryManager.synergyManager.humanGoldAmount;
            }
            goldToGive += incomesGenerated[i];
            if (round%2 == 0)
            {
                goldToGive += passiveGoldOnEvenRounds;
            }
            inventoryManager.AddGold((Player)i, goldToGive);
        }
        Debug.Log("Players have earned { " + incomesGenerated[0] + ", " + incomesGenerated[1] + ", " + +incomesGenerated[2] + " }");
        round++;
    }

    public void SetIncomeGeneratedByPlayer(Player player, int incomeGenerated)
    {
        incomesGenerated[(int) player] = incomeGenerated;
    }
}
