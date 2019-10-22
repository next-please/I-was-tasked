using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    public TransactionManager transactionManager;
    public GameObject UpgradeArmy;
    public GameObject UpgradeMarketRarity;
    public GameObject UpgradeMarketSize;

    Button rarityButton;
    Button sizeButton;
    Button armyButton;

    void OnEnable()
    {
        EventManager.Instance.AddListener<MarketUpdateEvent>(UpdateMarketRarityButtonsText);
        EventManager.Instance.AddListener<InventoryChangeEvent>(UpdateArmySizeButtonsText);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<MarketUpdateEvent>(UpdateMarketRarityButtonsText);
        EventManager.Instance.AddListener<InventoryChangeEvent>(UpdateArmySizeButtonsText);
    }

    void Awake()
    {
        SetMarketRarityButtons();
        SetArmySizeButtons();
        SetMarketSizeButtons();
        UpdateMarketRarityButtonsText(new MarketUpdateEvent());
        UpdateArmySizeButtonsText(new InventoryChangeEvent());
    }

    void UpdateMarketRarityButtonsText(MarketUpdateEvent e /*unused and is a hack*/)
    {
        if (rarityButton == null)
            return;
        TextMeshProUGUI text = rarityButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = transactionManager.GetMarketRarityCost().ToString();
    }

    void UpdateArmySizeButtonsText(InventoryChangeEvent e /*unused and is a hack*/)
    {
        if (armyButton == null)
            return;
        TextMeshProUGUI text = armyButton.GetComponentInChildren<TextMeshProUGUI>();
        Player localPlayer = RoomManager.GetLocalPlayer();
        text.text = transactionManager.GetArmySizeCost(localPlayer).ToString();
    }

    void SetMarketRarityButtons()
    {
        rarityButton = UpgradeMarketRarity.GetComponentInChildren<Button>(true);
        rarityButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseMarketRarity(RoomManager.GetLocalPlayer()));
    }

    void SetMarketSizeButtons()
    {
        sizeButton = UpgradeMarketSize.GetComponentInChildren<Button>(true);
        sizeButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseMarketSize(RoomManager.GetLocalPlayer()));
        TextMeshProUGUI text = sizeButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = transactionManager.UpgradeMarketSizeCost.ToString();
    }

    void SetArmySizeButtons()
    {
        armyButton = UpgradeArmy.GetComponentInChildren<Button>(true);
        armyButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseArmySize(RoomManager.GetLocalPlayer()));
    }
}
