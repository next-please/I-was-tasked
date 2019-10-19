using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;

public class MarketUIManager : MonoBehaviour
{
    public Canvas upgradeCanvas;
    public Canvas marketCanvas;
    public Canvas marketInfoCanvas;
    public Canvas marketTooltipCanvas;

    public Text MarketSizeText;
    public Text MarketRarityText;
    public Text PassiveIncomeText;
    public Text CastleHealthText;

    public MarketTooltip marketTooltip;

    public TransactionManager transactionManager;

    Button[] marketItemsButtons;
    IReadOnlyList<Piece> marketPieces;
    private bool visibility = true;

    // world view market
    public CharacterPrefabLoader characterPrefabLoader;
    public GameObject marketObject;
    MarketSlot[] marketSlots;

    private void Update()
    {
        if (Input.GetButtonDown("Toggle Market"))
        {
            SetCanvasVisibility(!visibility);
        }
    }
    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
        EventManager.Instance.AddListener<MarketUpdateEvent>(OnMarketUpdate);
        EventManager.Instance.AddListener<PurchaseMarketItemEvent>(OnPurchaseMarketItem);
        EventManager.Instance.AddListener<HoverMarketItemEvent>(OnHoverMarketItem);
        EventManager.Instance.AddListener<CameraPanEvent>(OnCameraPan);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
        EventManager.Instance.RemoveListener<MarketUpdateEvent>(OnMarketUpdate);
        EventManager.Instance.RemoveListener<PurchaseMarketItemEvent>(OnPurchaseMarketItem);
        EventManager.Instance.RemoveListener<HoverMarketItemEvent>(OnHoverMarketItem);
        EventManager.Instance.RemoveListener<CameraPanEvent>(OnCameraPan);
    }

    void Awake()
    {
        marketPieces = new List<Piece>();
        marketItemsButtons = marketCanvas.GetComponentsInChildren<Button>(true);
        marketSlots = marketObject.GetComponentsInChildren<MarketSlot>();

        ClearMarketButtons();
        for (int i = 0; i < marketItemsButtons.Length; ++i)
        {
            int capturedIndex = i;
            Button marketItemButton = marketItemsButtons[i];
            marketItemButton.onClick.AddListener(() => PurchasePiece(capturedIndex));
        }

        ClearMarket();

        SetCanvasVisibility(false);
        marketTooltipCanvas.enabled = false;
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            // SetCanvasVisibility(true);
        }
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            // SetCanvasVisibility(false);
        }
    }

    void OnCameraPan(CameraPanEvent e)
    {
        if (e.targetView == CameraView.Market)
        {
            ShowMarketUI();
        }
        else
        {
            HideMarketUI();
        }
    }

    void SetCanvasVisibility(bool visibility)
    {
        marketCanvas.enabled = visibility;
        upgradeCanvas.enabled = visibility;
        this.visibility = visibility;
    }

    void OnMarketUpdate(MarketUpdateEvent e)
    {
        MarketRarityText.text = "Market Rarity: Tier " + e.readOnlyMarket.GetMarketTier().ToString();
        MarketSizeText.text = "Market Size: " + e.readOnlyMarket.GetMarketSize().ToString();
        CastleHealthText.text = "Castle Health: " + e.readOnlyMarket.GetCastleHealth().ToString();
        UpdateMarketButtons(e);
        UpdateMarket(e);
    }

    void OnPurchaseMarketItem(PurchaseMarketItemEvent e)
    {
        Piece pieceToPurchase = e.piece;
        Player player = RoomManager.GetLocalPlayer();
        transactionManager.TryToPurchaseMarketPieceToBench(player, pieceToPurchase);
    }

    void UpdateMarketButtons(MarketUpdateEvent e)
    {
        ClearMarketButtons();
        marketPieces = e.readOnlyMarket.GetMarketPieces();
        for (int i = 0; i < marketPieces.Count; ++i)
        {
            Piece piece = marketPieces[i]; // please don't modify the piece here T_T
            if (piece == null)
            {
                continue;
            }
            Button marketItemButton = marketItemsButtons[i];
            marketItemButton.GetComponentInChildren<Text>().text =
                "<b>" + piece.GetTitle() + "</b>" +
                "\n<size=10>" + piece.GetName() + "</size>" +
                "\n" + piece.GetRace() + " (3)" +
                "    " + piece.GetClass() + " (3)" +
                "\nRarity: " + piece.GetRarity() +
                "  Cost: " + Math.Pow(2, piece.GetRarity() - 1);
            marketItemButton.enabled = true;
        }
    }

    void UpdateMarket(MarketUpdateEvent e)
    {
        ClearMarket();
        marketPieces = e.readOnlyMarket.GetMarketPieces();
        for (int i = 0; i < marketPieces.Count; ++i)
        {
            Piece piece = marketPieces[i]; // please don't modify the piece here T_T

            if (piece == null)
            {
                continue;
            }
            MarketSlot marketSlot = marketSlots[i];
            marketSlot.SetOccupant(piece, characterPrefabLoader.GetPrefab(piece));
        }
    }

    void ClearMarketButtons()
    {
        foreach (Button button in marketItemsButtons)
        {
            ClearButton(button);
        }
    }

    void ClearButton(Button button)
    {
        button.GetComponentInChildren<Text>().text = "-- Empty Market Slot --";
        button.enabled = false;
    }

    void ClearMarket()
    {
        foreach (MarketSlot slot in marketSlots)
        {
            slot.ClearSlot();
        }
    }

    void PurchasePiece(int itemIndex)
    {
        Piece pieceToPurchase = marketPieces[itemIndex];
        Player player = RoomManager.GetLocalPlayer();
        transactionManager.TryToPurchaseMarketPieceToBench(player, pieceToPurchase);
    }

    void OnHoverMarketItem(HoverMarketItemEvent e)
    {
        if (e.piece == null)
        {
            HideMarketTooltip();
        }
        else
        {
            ShowMarketTooltip(e.piece);
        }
    }

    private void HideMarketUI()
    {
        marketInfoCanvas.enabled = false;
        upgradeCanvas.enabled = false;
    }

    private void ShowMarketUI()
    {
        marketInfoCanvas.enabled = true;
        upgradeCanvas.enabled = true;
    }

    private void HideMarketTooltip()
    {
        if (marketTooltipCanvas)
        {
            marketTooltipCanvas.enabled = false;
        }
    }

    private void ShowMarketTooltip(Piece piece)
    {
        if (marketTooltipCanvas && marketTooltip)
        {
            marketTooltipCanvas.enabled = true;
            marketTooltip.SetMarketItemInfo(piece);
        }
    }
}
