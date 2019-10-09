using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;

public class UpgradeUIManager : MonoBehaviour
{
    public TransactionManager transactionManager;
    public GameObject UpgradeArmy;
    public GameObject UpgradeMarketRarity;
    public GameObject UpgradeMarketSize;
    public GameObject UpgradeIncome;

    Button rarityButton;
    Button sizeButton;
    Button incomeButton;
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
        SetIncomeButtons();
        SetMarketSizeButtons();

        UpdateMarketRarityButtonsText(new MarketUpdateEvent());
        UpdateArmySizeButtonsText(new InventoryChangeEvent());
    }

    void UpdateMarketRarityButtonsText(MarketUpdateEvent e /*unused and is a hack*/)
    {
        if (rarityButton == null)
            return;
        Text text = rarityButton.GetComponentInChildren<Text>();
        text.text = "Upgrade Market Rarity ($" + transactionManager.GetMarketRarityCost() + ")";
    }

    void UpdateArmySizeButtonsText(InventoryChangeEvent e /*unused and is a hack*/)
    {
        if (armyButton == null)
            return;
        Text text = armyButton.GetComponentInChildren<Text>();
        Player localPlayer = RoomManager.GetLocalPlayer();
        text.text = "Upgrade Army Size ($" + transactionManager.GetArmySizeCost(localPlayer) + ")";
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
        Text text = sizeButton.GetComponentInChildren<Text>();
        text.text = "Upgrade Market Size $(" + transactionManager.UpgradeMarketSizeCost + ")";
    }

    void SetIncomeButtons()
    {
        incomeButton = UpgradeIncome.GetComponentInChildren<Button>(true);
        incomeButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreasePassiveIncome(RoomManager.GetLocalPlayer()));
        Text text = incomeButton.GetComponentInChildren<Text>();
        text.text = "Upgrade Passive Income $(" + transactionManager.UpgradeIncomeCost + ")";
    }

    void SetArmySizeButtons()
    {
        armyButton = UpgradeArmy.GetComponentInChildren<Button>(true);
        armyButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseArmySize(RoomManager.GetLocalPlayer()));
    }
}
