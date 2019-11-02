using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    public GameObject BenchItemPrefab;
    public bool isOccupied;
    public int index;
    public BenchItem benchItem;

    public void SetOccupant(Piece piece, GameObject characterModel, Player player)
    {
        isOccupied = true;

        // create item prefab
        GameObject benchItemObj = Instantiate(BenchItemPrefab) as GameObject;
        benchItem = benchItemObj.GetComponent<BenchItem>();
        benchItem.piece = piece;
        benchItem.index = index;
        benchItem.InstantiateModelPrefab(characterModel);

        // set item prefab as child of slot
        benchItemObj.transform.SetParent(this.transform);
        benchItemObj.transform.localPosition = new Vector3(0, 0.5f, 0);
        float rotation = (int) player * 45;
        benchItemObj.transform.Rotate(new Vector3(0, 1, 0), rotation);
        benchItem.AdjustRarityRotationView(player);
    }

    public void SetEmpty()
    {
        isOccupied = false;
        if (benchItem)
            Destroy(benchItem.gameObject);
    }
}
