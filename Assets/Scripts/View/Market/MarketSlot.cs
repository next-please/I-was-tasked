using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketSlot : MonoBehaviour
{
    public GameObject MarketItemPrefab;
    public MarketItem marketItem;
    public bool isOccupied;

    public void SetOccupant(Piece piece, GameObject characterModel)
    {
        isOccupied = true;

        // create item prefab
        GameObject marketItemObj = Instantiate(MarketItemPrefab) as GameObject;
        marketItem = marketItemObj.GetComponent<MarketItem>();
        marketItem.piece = piece;
        marketItem.InstantiateModelPrefab(characterModel);

        // set item prefab as child of slot
        marketItemObj.transform.SetParent(this.transform);
        marketItemObj.transform.localPosition = new Vector3(0, 0.5f, 0);
        marketItemObj.transform.rotation = this.transform.rotation;
    }

    public void ClearSlot()
    {
        isOccupied = false;
        if (marketItem)
            Destroy(marketItem.gameObject);
    }
}
