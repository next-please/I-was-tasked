using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Com.Nextplease.IWT;
using TMPro;

public class MarketUIManager : MonoBehaviour
{
    public Canvas marketCanvas;
    public Canvas marketInfoCanvas;

    public TextMeshProUGUI MarketSizeText;
    public TextMeshProUGUI MarketRarityText;

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
        EventManager.Instance.AddListener<MarketUpdateEvent>(OnMarketUpdate);
        EventManager.Instance.AddListener<PurchaseMarketItemEvent>(OnPurchaseMarketItem);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<MarketUpdateEvent>(OnMarketUpdate);
        EventManager.Instance.RemoveListener<PurchaseMarketItemEvent>(OnPurchaseMarketItem);
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
    }

    void SetCanvasVisibility(bool visibility)
    {
        marketCanvas.enabled = visibility;
        this.visibility = visibility;
    }

    void OnMarketUpdate(MarketUpdateEvent e)
    {
        MarketRarityText.text = e.readOnlyMarket.GetMarketTier().ToString();
        MarketSizeText.text = e.readOnlyMarket.GetMarketSize().ToString();
        UpdateMarketButtons(e);
        UpdateMarket(e);
    }

    void OnPurchaseMarketItem(PurchaseMarketItemEvent e)
    {
        Piece pieceToPurchase = e.piece;
        Player player = RoomManager.GetLocalPlayer();
        transactionManager.TryToPurchaseMarketPieceToBench(player, pieceToPurchase);
        SoundManager.Instance.PlayPieceSound("Purchase");
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
}
