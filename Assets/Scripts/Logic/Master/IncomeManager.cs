using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncomeUpdateEvent : GameEvent
{
    public int PassiveIncome;
}

public class IncomeManager : MonoBehaviour
{
    public int MaxFixedIncome = 5;
    [SerializeField]
    private int incomeFromUpgrades = 0;
    public readonly decimal InterestRate = 1m / 10;
    public InventoryManager inventoryManager;

    public void GenerateIncome(int currentRound)
    {
        // if I'm master client
        int currentIncome = Math.Min(currentRound, MaxFixedIncome);
        currentIncome += incomeFromUpgrades;
        for (int i = 0; i < 3; ++i)
        {
            Player player = (Player) i;
            int playerGold = inventoryManager.GetGold(player);
            decimal incomeFromInterest = Math.Floor(playerGold * InterestRate);
            int income = currentIncome + Convert.ToInt32(incomeFromInterest);
            inventoryManager.AddGold(player, income);
        }
        // for now
        EventManager.Instance.Raise(new PassiveIncomeUpdateEvent{ PassiveIncome = incomeFromUpgrades });
    }

    public void IncreasePassiveIncome()
    {
        incomeFromUpgrades++;

        //retroactive upgrades
        int income = 1;
        for (int i = 0; i < 3; ++i)
        {
            Player player = (Player)i;
            inventoryManager.AddGold(player, income);
        }

        EventManager.Instance.Raise(new GlobalMessageEvent { message = "Passive Income Purchased! Everyone received 1 gold." });
        EventManager.Instance.Raise(new PassiveIncomeUpdateEvent{ PassiveIncome = incomeFromUpgrades });
    }
}
