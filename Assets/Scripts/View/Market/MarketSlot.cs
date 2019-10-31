using System;
using UnityEngine;

public class MarketSlot : MonoBehaviour
{
    public GameObject MarketItemPrefab;
    public MarketItem marketItem;
    public bool isOccupied;

    public Material[] materials;
    public GameObject price;
    public GameObject[] rarities;
    public GameObject raritiesParent;

    private void Start()
    {
        price.transform.LookAt(price.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        price.SetActive(false);
        raritiesParent.transform.LookAt(raritiesParent.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        foreach (GameObject rarity in rarities)
        {
            rarity.SetActive(false);
        }
    }

    public void SetOccupant(Piece piece, GameObject characterModel)
    {
        isOccupied = true;
        price.SetActive(true);
        price.GetComponent<TextMesh>().text = string.Format("{0}", (int) Math.Pow(2, piece.GetRarity() - 1));
        rarities[piece.GetRarity() - 1].SetActive(true);

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
        price.SetActive(false);
        foreach (GameObject rarity in rarities)
        {
            rarity.SetActive(false);
        }
        if (marketItem)
            Destroy(marketItem.gameObject);
    }

    public void SetActiveSlot(bool isActive)
    {
        GetComponent<Renderer>().material = materials[(isActive) ? 1 : 0];
    }
}
