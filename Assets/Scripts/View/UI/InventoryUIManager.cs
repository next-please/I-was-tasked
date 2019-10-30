using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public Canvas CurrentArmySizeCanvas;

    public TextMeshProUGUI CurrentArmySizeText;
    public TextMeshProUGUI ArmySizeText;
    public TextMeshProUGUI GoldText;

    void Awake()
    {
        CurrentArmySizeCanvas.enabled = false;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<InventoryChangeEvent>(OnInventoryChange);
        EventManager.Instance.AddListener<CameraPanEvent>(OnCameraPan);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<InventoryChangeEvent>(OnInventoryChange);
        EventManager.Instance.RemoveListener<CameraPanEvent>(OnCameraPan);
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
            CurrentArmySizeText.text = inv.GetArmyCount() + "/" + inv.GetArmySize();
        }
    }

    void OnCameraPan(CameraPanEvent e)
    {
        if (CameraController.IsViewingOwnBoard())
        {
            CurrentArmySizeCanvas.enabled = true;
        }
        else
        {
            CurrentArmySizeCanvas.enabled = false;
        }
    }
}
