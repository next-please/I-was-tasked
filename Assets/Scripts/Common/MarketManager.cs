using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public Canvas upgradeCanvas;
    public Canvas marketCanvas;
    public int StartingMarketSize = 5;
    public PlayerInventory playerInventory;

    Piece[] marketItems;
    int marketSize;
    int marketTier = 1; //also used as price for market increase
    CharacterGenerator characterGenerator;
    Button[] marketItemsButtons;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
    }

    void Awake()
    {
        characterGenerator = new CharacterGenerator();
        marketSize = StartingMarketSize;
        SetUpMarketItemButtons();
    }

    void SetUpMarketItemButtons()
    {

        marketItemsButtons = marketCanvas.GetComponentsInChildren<Button>(true);
        ClearMarketButtons();

        for (int i = 0; i < marketItemsButtons.Length; ++i)
        {
            int capturedIndex = i;
            Button marketItemButton = marketItemsButtons[i];
            marketItemButton.onClick.AddListener(() => Purchase(capturedIndex));
        }
    }

    void SetCanvasVisibility(bool visibility)
    {
        marketCanvas.enabled = visibility;
        upgradeCanvas.enabled = visibility;
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            GenerateAndDisplayMarket();
        }
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            SetCanvasVisibility(false);
        }
    }

    void GenerateAndDisplayMarket()
    {
        SetCanvasVisibility(true);
        ClearMarketButtons();
        GenerateMarketItems();
        UpdateMarketButtons();
    }

    void ClearMarketButtons()
    {
        foreach (Button button in marketItemsButtons)
        {
            ClearButton(button);
        }
    }

    void GenerateMarketItems()
    {
        marketItems = new Piece[marketSize];
        for (int i = 0; i < marketSize; ++i)
        {
            Piece piece = characterGenerator.GenerateCharacter(marketTier);
            marketItems[i] = piece;
        }
    }

    void UpdateMarketButtons()
    {
        for (int i = 0; i < marketSize; ++i)
        {
            Piece piece = marketItems[i];
            Button marketItemButton = marketItemsButtons[i];
            marketItemButton.GetComponentInChildren<Text>().text =
                piece.GetName() +
                "\nRace: " + piece.GetRace() +
                "\nJob: " + piece.GetClass() +
                "\nRarity and Cost: " + piece.GetRarity();
            marketItemButton.enabled = true;
        }
    }

    void ClearButton(Button button)
    {
        button.GetComponentInChildren<Text>().text = "-- Empty Market Slot --";
        button.enabled = false;
    }

    void Purchase(int itemIndex)
    {
        Piece pieceToPurchase = marketItems[itemIndex];
        int price = pieceToPurchase.GetRarity();
        if (playerInventory.IsGarrisonFull())
            return;
        bool managedToBuy = playerInventory.TryToPurchase(price);
        if (managedToBuy)
        {
            playerInventory.AddToGarrison(pieceToPurchase);
            Button button = marketItemsButtons[itemIndex];
            ClearButton(button);
        }
    }
}
