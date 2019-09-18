using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class BenchManager : MonoBehaviour
{
    private readonly int maxSlots = 8;
    public float benchSize = 1f;
    public Player player;
    public GameObject BenchSlotPrefab;
    public GameObject BenchItemPrefab;
    public CharacterPrefabLoader characterPrefabLoader;

    private List<BenchSlot> slots = new List<BenchSlot>();
    private int nextEmptySlotIndex = 0;

    void OnEnable()
    {
        EventManager.Instance.AddListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<InventoryChangeEvent>(OnInventoryChange);
    }

    void Awake()
    {
        // init bench slots
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject benchSlotObj = Instantiate(BenchSlotPrefab) as GameObject;

            benchSlotObj.transform.SetParent(this.transform);
            benchSlotObj.transform.localPosition = new Vector3(i, 0, 0) * benchSize;

            BenchSlot slot = benchSlotObj.GetComponent<BenchSlot>();
            slot.index = i;
            slots.Add(slot);
        }
    }

    public void OnInventoryChange(InventoryChangeEvent e)
    {
        var playerInv = e.inventory;
        if (playerInv.GetOwner() != RoomManager.GetLocalPlayer())
        {
            return;
        }
        var bench = playerInv.GetBench();

        for (int i = 0; i < bench.Count; ++i)
        {
            Piece benchPiece = bench[i];
            SetPiece(benchPiece, i);
        }
    }

    private void SetPiece(Piece piece, int index)
    {
        if (index >= slots.Count)
        {
            return;
        }
        var slot = slots[index];
        if (piece == null)
        {
            slot.SetEmpty();
            return;
        }
        if (slot.isOccupied)
        {
            slot.benchItem.piece = piece;
            return;
        }
        GameObject characterPrefab = characterPrefabLoader.GetPrefab(piece);
        slots[index].SetOccupant(piece, characterPrefab);
    }
}
