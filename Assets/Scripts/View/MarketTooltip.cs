using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MarketTooltip : MonoBehaviour
{
    private TextMeshProUGUI info;

    void Awake()
    {
        info = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }

    public void SetMarketItemInfo(Piece piece)
    {
        info.text = "<b>" + piece.GetTitle() + "</b>" +
            "\n<size=10>" + piece.GetName() + "</size>" +
            "\n" + piece.GetRace() + " (3)" +
            "    " + piece.GetClass() + " (3)" +
            "\nRarity: " + piece.GetRarity() +
            "  Cost: " + Math.Pow(2, piece.GetRarity() - 1);
    }
}
