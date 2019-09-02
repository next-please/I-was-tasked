using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BenchManager : MonoBehaviour
{
    private readonly int maxSlots = 8;

    private List<BenchSlot> slots = new List<BenchSlot>();
    private int nextEmptySlotIndex = 0;
    private int itemCount = 0;

    public GameObject BenchSlotPrefab;
    public GameObject BenchItemPrefab;

    void OnEnable()
    {
        EventManager.Instance.AddListener<RemovePieceFromBenchEvent>(OnPieceRemoved);
        EventManager.Instance.AddListener<AddPieceToBenchEvent>(OnPieceAdded);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<RemovePieceFromBenchEvent>(OnPieceRemoved);
        EventManager.Instance.RemoveListener<AddPieceToBenchEvent>(OnPieceAdded);
    }

    void Start()
    {
        // init bench slots
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject benchSlotObj = Instantiate(BenchSlotPrefab) as GameObject;

            benchSlotObj.transform.SetParent(this.transform);
            benchSlotObj.transform.localPosition = Vector3.zero;
            benchSlotObj.transform.localScale = Vector3.one;
            benchSlotObj.transform.rotation = transform.rotation;

            BenchSlot slot = benchSlotObj.GetComponent<BenchSlot>();
            slot.index = i;
            slots.Add(slot);
        }

        // testing
        Piece lewis_enemy = new Piece("Lewis the Jesus Koh", 100, 10, 3, true);
        Piece junkai_enemy = new Piece("Jun the Supreme Kai", 100, 20, 1, true);
        Piece jolyn_player = new Piece("Jo Jo Lyn", 100, 25, 1, false);
        Piece nicholas_player = new Piece("Nick Pepega Chua", 100, 30, 4, false);
        AddPiece(lewis_enemy, nextEmptySlotIndex);
        AddPiece(junkai_enemy, nextEmptySlotIndex);
        AddPiece(jolyn_player, nextEmptySlotIndex);
        AddPiece(nicholas_player, nextEmptySlotIndex);
    }

    public void AddPieceTest()
    {
        Piece lewis_enemy = new Piece("Lewis the Jesus Koh", 100, 10, 3, true);
        AddPiece(lewis_enemy, nextEmptySlotIndex);
    }

    public void OnPieceRemoved(RemovePieceFromBenchEvent e)
    {
        RemovePiece(e.slotIndex);
    }

    public void OnPieceAdded(AddPieceToBenchEvent e)
    {
        AddPiece(e.piece, e.slotIndex);
    }

    private void AddPiece(Piece piece, int index)
    {
        if (itemCount >= maxSlots)
        {
            return;
        }

        slots[index].SetOccupant(piece);
        itemCount++;
        UpdateNextEmptySlotIndex();
    }

    private void RemovePiece(int index)
    {
        if (itemCount == 0)
        {
            return;
        }

        slots[index].SetEmpty();
        itemCount--;
        UpdateNextEmptySlotIndex();
    }

    private void UpdateNextEmptySlotIndex()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (!slots[i].isOccupied)
            {
                nextEmptySlotIndex = i;
                return;
            }
        }
    }
}

public class RemovePieceFromBenchEvent : GameEvent
{
    public int slotIndex;
}

public class AddPieceToBenchEvent : GameEvent
{
    public Piece piece;
    public int slotIndex;
}
