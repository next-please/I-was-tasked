using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseMarketItemEvent : GameEvent
{
    public Piece piece;
    public int index;
}

public class HoverMarketItemEvent : GameEvent
{
    public Piece piece;
}

public class MarketItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Piece piece;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: change cursor? (with add/buy icon)
        EventManager.Instance.Raise(new HoverMarketItemEvent { piece = piece });

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Instance.Raise(new HoverMarketItemEvent { piece = null });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.Instance.Raise(new PurchaseMarketItemEvent { piece = piece });
    }

    public void InstantiateModelPrefab(GameObject characterModel)
    {
        GameObject modelPrefab = Instantiate(characterModel) as GameObject;
        modelPrefab.transform.SetParent(this.transform);
        modelPrefab.transform.localPosition = Vector3.zero;
        modelPrefab.transform.localScale = Vector3.one;
    }
}
