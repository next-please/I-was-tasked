using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private int[] incomesGenerated = { 0, 0, 0 }; 

    public void GenerateIncome()
    {
        // If I'm master client
        for (int i = 0; i < 3; ++i)
        {
            inventoryManager.AddGold((Player) i, incomesGenerated[i]);
            
        }
        Debug.Log("Players have earned { " + incomesGenerated[0] + ", " + incomesGenerated[1] + ", " + +incomesGenerated[2] + " }");
    }

    public void SetIncomeGeneratedByPlayer(Player player, int incomeGenerated)
    {
        incomesGenerated[(int) player] = incomeGenerated;
    }
}
