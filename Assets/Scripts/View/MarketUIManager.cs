﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MarketUIManager : MonoBehaviour
{
    public Canvas upgradeCanvas;
    public Canvas marketCanvas;

    public Text MarketSizeText;
    public Text MarketRarityText;
    public Text PassiveIncomeText;

    public TransactionManager transactionManager;

    Button[] marketItemsButtons;
    IReadOnlyList<Piece> marketPieces;
    private bool visibility = true;

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
        EventManager.Instance.AddListener<PassiveIncomeUpdateEvent>(OnPassiveIncomeUpdate);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
        EventManager.Instance.RemoveListener<MarketUpdateEvent>(OnMarketUpdate);
        EventManager.Instance.RemoveListener<PassiveIncomeUpdateEvent>(OnPassiveIncomeUpdate);
    }

    void Awake()
    {
        marketPieces = new List<Piece>();
        marketItemsButtons = marketCanvas.GetComponentsInChildren<Button>(true);
        ClearMarketButtons();
        for (int i = 0; i < marketItemsButtons.Length; ++i)
        {
            int capturedIndex = i;
            Button marketItemButton = marketItemsButtons[i];
            marketItemButton.onClick.AddListener(() => PurchasePiece(capturedIndex));
        }
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            SetCanvasVisibility(true);
        }
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            SetCanvasVisibility(false);
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
        UpdateMarketButtons(e);
    }

    void OnPassiveIncomeUpdate(PassiveIncomeUpdateEvent e)
    {
        PassiveIncomeText.text = "Additional Passive Income: "  + e.PassiveIncome.ToString();
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
                piece.GetName() +
                "\nRace: " + piece.GetRace() +
                "\nJob: " + piece.GetClass() +
                "\nRarity: " + piece.GetRarity() +
                "  Cost: " + Math.Pow(2, piece.GetRarity()-1);
            marketItemButton.enabled = true;
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

    void PurchasePiece(int itemIndex)
    {
        Piece pieceToPurchase = marketPieces[itemIndex];
        // TODO: FIX THIS!
        Player player = Player.Zero;
        transactionManager.TryToPurchaseMarketPieceToBench(player, pieceToPurchase);
    }


}
