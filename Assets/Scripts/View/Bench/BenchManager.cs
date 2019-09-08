using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchManager : MonoBehaviour
{
    private readonly int maxSlots = 8;
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
            benchSlotObj.transform.localPosition = Vector3.zero;
            benchSlotObj.transform.localScale = Vector3.one;
            benchSlotObj.transform.rotation = transform.rotation;

            BenchSlot slot = benchSlotObj.GetComponent<BenchSlot>();
            slot.index = i;
            slots.Add(slot);
        }
    }

    public void OnInventoryChange(InventoryChangeEvent e)
    {
        var playerInv = e.inventory;
        if (playerInv.GetOwner() != player)
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
            return;
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
