using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public TransactionManager transactionManager;
    public GameObject UpgradeArmy;
    public GameObject UpgradeMarketRarity;
    public GameObject UpgradeMarketSize;
    public GameObject UpgradeIncome;

    Button[] rarityButtons;
    Button[] sizeButtons;
    Button[] incomeButtons;
    Button[] armyButtons;

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

    // Start is called before the first frame update
    void Start()
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
        if (rarityButtons == null)
            return;
        for (int i = 0; i < 3; ++i)
        {
            Button button = rarityButtons[i];
            Text text = button.GetComponentInChildren<Text>();
            text.text = "Upgrade Market Rarity ($" + transactionManager.GetMarketRarityCost() + ")";
        }
    }

    void UpdateArmySizeButtonsText(InventoryChangeEvent e /*unused and is a hack*/)
    {
        if (armyButtons == null)
            return;
        for (int i = 0; i < 3; ++i)
        {
            Button button = armyButtons[i];
            Player player = (Player) i;
            Text text = button.GetComponentInChildren<Text>();
            text.text = "Upgrade Army Size ($" + transactionManager.GetArmySizeCost(player) + ")";
        }
    }

    void SetMarketRarityButtons()
    {
        rarityButtons = UpgradeMarketRarity.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < rarityButtons.Length; ++i)
        {
            Button rarityButton = rarityButtons[i];
            Player player = (Player) i;
            rarityButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseMarketRarity(player));
        }
    }

    void SetMarketSizeButtons()
    {
        sizeButtons = UpgradeMarketSize.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < sizeButtons.Length; ++i)
        {
            Button sizeButton = sizeButtons[i];
            Player player = (Player) i;
            sizeButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseMarketSize(player));
            Text text = sizeButton.GetComponentInChildren<Text>();
            text.text = "Upgrade Market Size $(" + transactionManager.UpgradeMarketSizeCost + ")";
        }
    }

    void SetIncomeButtons()
    {
        incomeButtons = UpgradeIncome.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < incomeButtons.Length; ++i)
        {
            Button incomeButton = incomeButtons[i];
            Player player = (Player) i;
            incomeButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreasePassiveIncome(player));
            Text text = incomeButton.GetComponentInChildren<Text>();
            text.text = "Upgrade Passive Income $(" + transactionManager.UpgradeIncomeCost + ")";
        }
    }

    void SetArmySizeButtons()
    {
        armyButtons = UpgradeArmy.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < armyButtons.Length; ++i)
        {
            Button armyButton = armyButtons[i];
            Player player = (Player) i;
            armyButton.onClick.AddListener(() => transactionManager.TryPurchaseIncreaseArmySize(player));
        }
    }

}
