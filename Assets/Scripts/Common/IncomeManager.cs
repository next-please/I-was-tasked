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
    public List<PlayerInventory> pInventories;

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
        int currentIncome = Math.Min(currentRound, MaxFixedIncome);
        currentIncome += IncomeFromUpgrades;
        foreach (PlayerInventory inventory in pInventories)
        {
            decimal incomeFromInterest = Math.Floor(inventory.GetGold() * InterestRate);
            int income = currentIncome + Convert.ToInt32(incomeFromInterest);
            inventory.AddGold(income);
        }
    }
}
