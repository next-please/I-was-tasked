using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : Droppable
{
    private readonly float distanceOffset = 10f;
    private readonly float scaleOffset = 10f;

    public Piece piece;
    public int index;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale /= scaleOffset; // update to world scale

        EventManager.Instance.Raise(new PieceDragEvent { piece = piece });
    }

    public override void OnDrag(PointerEventData eventData)
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = distanceOffset;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public override void OnBenchDrop(BenchSlot targetSlot)
    {
        EventManager.Instance.Raise(new AddPieceToBenchEvent
        {
            piece = piece,
            slotIndex = targetSlot.index
        });
        EventManager.Instance.Raise(new RemovePieceFromBenchEvent { slotIndex = index });
        Destroy(gameObject);
    }

    public override void OnTileDrop(Tile tile)
    {
        if (tile.IsOccupied())
        {
            return;
        }
        EventManager.Instance.Raise(new PieceDropOnBoardEvent { tile = tile });
        EventManager.Instance.Raise(new RemovePieceFromBenchEvent { slotIndex = index });
        Destroy(gameObject);
    }

    public override void OnTrashDrop()
    {
        EventManager.Instance.Raise(new RemovePieceFromBenchEvent { slotIndex = index });
        Destroy(gameObject);
    }

    // Returns piece to bench
    public override void OnEmptyDrop()
    {
        transform.localScale *= scaleOffset; // update to ui scale
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
