using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    private const int MAX_SLOTS = 8;
    private List<BenchItem> items;

    public GameObject BenchItemPrefab;

    void Start()
    {
        items = new List<BenchItem>();
        // testing
        AddItem(new MeleePiece("Lewis the Jesus Koh", 100, 1, true));
        AddItem(new MeleePiece("Jun the Supreme Kai", 100, 2, true));
        AddItem(new MeleePiece("Jo Jo Lyn", 100, 3, false));
        AddItem(new MeleePiece("Nick Pepega Chua", 100, 4, false));
    }

    public void AddItem(Piece piece)
    {
        if (items.Count >= MAX_SLOTS)
        {
            return;
        }

        GameObject benchItemObj = Instantiate(BenchItemPrefab) as GameObject;
        BenchItem benchItem = benchItemObj.GetComponent<BenchItem>();
        benchItem.SetPiece(piece);

        Transform benchSlot = transform.GetChild(items.Count);
        benchItemObj.transform.SetParent(benchSlot);
        benchItemObj.transform.localPosition = Vector3.zero;

        items.Add(benchItem);
    }

    public void RemoveItem(BenchItem item)
    {
        // TODO: remove from view
        items.Remove(item);
    }
}
