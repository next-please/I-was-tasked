using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardTooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardTooltipInfo;

    public void Awake()
    {
        cardTooltipInfo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        cardTooltipInfo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        cardTooltipInfo.SetActive(false);
    }
}
