﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public TextMeshProUGUI CurrentArmySizeText;
    public TextMeshProUGUI ArmySizeText;
    public TextMeshProUGUI GoldText;

    void OnEnable()
    {
        EventManager.Instance.AddListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void OnInventoryChange(InventoryChangeEvent e)
    {
        PlayerInventory inv = e.inventory;
        Player owner = inv.GetOwner();

        int index = (int)owner; // TODO: Fix/Address assumption
        if (RoomManager.GetLocalPlayer() == owner)
        {
            GoldText.text = inv.GetGold().ToString();
            ArmySizeText.text = inv.GetArmySize().ToString();
            if (inv.GetArmyCount() > inv.GetArmySize())
            {
                CurrentArmySizeText.text = "<color=\"red\">" + "<size=120%>" + inv.GetArmyCount() + "</size>" + "</color>" + "/" + inv.GetArmySize();
            }
            else
            {
                CurrentArmySizeText.text = inv.GetArmyCount() + "/" + inv.GetArmySize();
            }
        }
    }
}
