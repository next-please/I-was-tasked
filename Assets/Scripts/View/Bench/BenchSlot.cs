using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    public GameObject BenchItemPrefab;
    public bool isOccupied;
    public int index;
    public BenchItem benchItem;

    public void SetOccupant(Piece piece)
    {
        isOccupied = true;

        // create item prefab
        GameObject benchItemObj = Instantiate(BenchItemPrefab) as GameObject;
        benchItem = benchItemObj.GetComponent<BenchItem>();
        benchItem.piece = piece;
        benchItem.index = index;

        // set item prefab as child of slot
        benchItemObj.transform.SetParent(this.transform);
        benchItemObj.transform.localPosition = Vector3.zero;
    }

    public void SetEmpty()
    {
        isOccupied = false;
        if (benchItem)
            Destroy(benchItem.gameObject);
    }
}
