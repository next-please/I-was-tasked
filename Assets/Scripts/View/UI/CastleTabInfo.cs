using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CastleTabInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject castleHealth;
    public TextMeshProUGUI _description;
    public MarketManager marketManager;

    public void Awake()
    {
        castleHealth.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        _description.SetText(marketManager.GetCastleHealth() + " / " + MarketManager.StartingCastleHealth);
        castleHealth.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        castleHealth.SetActive(false);
    }

}
