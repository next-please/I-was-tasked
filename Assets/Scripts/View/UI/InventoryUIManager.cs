using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;

public class InventoryUIManager : MonoBehaviour
{
    public Text PlayerName;
    public Text GoldText;
    public Text ArmySizeText;

    void OnEnable()
    {
        EventManager.Instance.AddListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void Start()
    {
        PlayerName.text = RoomManager.GetLocalPlayerNickname();
    }

    void OnInventoryChange(InventoryChangeEvent e)
    {
        PlayerInventory inv = e.inventory;
        Player owner = inv.GetOwner();

        int index = (int) owner; // TODO: Fix/Address assumption
        if (RoomManager.GetLocalPlayer() == owner)
        {
            GoldText.text = inv.GetGold() + " Gold";
            ArmySizeText.text = "Army Size: " + inv.GetArmyCount() + "/" + inv.GetArmySize();
        }
    }
}
