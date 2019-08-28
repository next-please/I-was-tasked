using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    private const int MAX_SLOTS = 8;

    // private List<BenchItem> items;
    private List<bool> isOccupied;
    private int nextEmptySlotIndex = 0;
    private int itemCount = 0;

    public GameObject BenchItemPrefab;

    void Start()
    {
        isOccupied = new List<bool>();
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            isOccupied.Add(false);
        }

        // testing
        AddItem(new MeleePiece("Lewis the Jesus Koh", 100, 1, true));
        AddItem(new MeleePiece("Jun the Supreme Kai", 100, 2, true));
        AddItem(new MeleePiece("Jo Jo Lyn", 100, 3, false));
        AddItem(new MeleePiece("Nick Pepega Chua", 100, 4, false));
    }

    public void AddItem(Piece piece)
    {
        if (itemCount >= MAX_SLOTS)
        {
            return;
        }

        // create item prefab
        GameObject benchItemObj = Instantiate(BenchItemPrefab) as GameObject;
        BenchItem benchItem = benchItemObj.GetComponent<BenchItem>();
        benchItem.SetPiece(piece);
        benchItem.SetIndex(nextEmptySlotIndex);

        // set item prefab as child of slot
        Transform slot = transform.GetChild(nextEmptySlotIndex);
        benchItemObj.transform.SetParent(slot);
        benchItemObj.transform.localPosition = Vector3.zero;

        // update model
        isOccupied[nextEmptySlotIndex] = true;
        itemCount++;
        UpdateNextEmptySlotIndex();
    }

    public void RemoveItem(int index)
    {
        if (itemCount == 0)
        {
            return;
        }

        // TODO: remove from view


        // update model
        isOccupied[index] = false;
        itemCount--;
        UpdateNextEmptySlotIndex();
    }

    private void UpdateNextEmptySlotIndex()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (!isOccupied[i])
            {
                nextEmptySlotIndex = i;
                return;
            }
        }
    }
}
