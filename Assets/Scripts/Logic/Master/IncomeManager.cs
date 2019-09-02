using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : MonoBehaviour
{
    public int MaxFixedIncome = 5;
    public int IncomeFromUpgrades = 1;
    public readonly decimal InterestRate = 1m / 10;
    public InventoryManager inventoryManager;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Initialization || e.phase == Phase.PostCombat)
        {
            GenerateIncome(e.round);
        }
    }

    void GenerateIncome(int currentRound)
    {
        // if I'm master client
        int currentIncome = Math.Min(currentRound, MaxFixedIncome);
        currentIncome += IncomeFromUpgrades;
        for (int i = 0; i < 3; ++i)
        {
            Player player = (Player) i;
            int playerGold = inventoryManager.GetGold(player);
            decimal incomeFromInterest = Math.Floor(playerGold * InterestRate);
            int income = currentIncome + Convert.ToInt32(incomeFromInterest);
            inventoryManager.AddGold(player, income);
        }
    }
}
