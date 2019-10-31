using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private int[] incomesGenerated = { 1, 1, 1 };
    private int round = 0; // Players earn end-round goal equal to the round number.

    public void GenerateIncome()
    {
        // If I'm master client
        for (int i = 0; i < 3; ++i)
        {
            if (inventoryManager.synergyManager.HasSynergy(Enums.Race.Human))
            {
                inventoryManager.AddGold((Player)i, round + incomesGenerated[i] + inventoryManager.synergyManager.humanGoldAmount);
            }
            else
            {
                inventoryManager.AddGold((Player)i, round + incomesGenerated[i]);
            }
        }
        Debug.Log("Players have earned { " + incomesGenerated[0] + ", " + incomesGenerated[1] + ", " + +incomesGenerated[2] + " }");
        round++;
    }

    public void SetIncomeGeneratedByPlayer(Player player, int incomeGenerated)
    {
        incomesGenerated[(int) player] = incomeGenerated;
    }
}
