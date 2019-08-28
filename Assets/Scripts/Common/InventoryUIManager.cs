using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public Text[] GoldTexts;
    public Text[] ArmySizeTexts;

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

        int index = (int) owner; // TODO: Fix/Address assumption
        GoldTexts[index].text = inv.GetGold() + " Gold";
        ArmySizeTexts[index].text = "Army Size: " + inv.GetGarrisonCount() + "/" + inv.GetArmySize();
    }
}
