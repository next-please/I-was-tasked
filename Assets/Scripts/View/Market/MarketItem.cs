using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseMarketItemEvent : GameEvent
{
    public Piece piece;
    public int index;
}

public class MarketItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject pieceInfo;
    public Piece piece;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Here");
        // TODO: show info
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exit");
        // TODO: hide info
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
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
