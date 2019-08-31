﻿using System.Collections;
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
        Piece lewis_enemy = new Piece("Lewis the Jesus Koh", 100, 10, 3, true);
        Piece junkai_enemy = new Piece("Jun the Supreme Kai", 100, 20, 1, true);
        Piece jolyn_player = new Piece("Jo Jo Lyn", 100, 25, 1, false);
        Piece nicholas_player = new Piece("Nick Pepega Chua", 100, 30, 4, false);

        AddItem(lewis_enemy);
        AddItem(junkai_enemy);
        AddItem(jolyn_player);
        AddItem(nicholas_player);
    }

    public void AddPieceTest()
    {
        Piece lewis_enemy = new Piece("Lewis the Jesus Koh", 100, 10, 3, true);
        AddItem(lewis_enemy);
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
        benchItem.SetBench(this);

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
