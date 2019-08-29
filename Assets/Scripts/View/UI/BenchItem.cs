using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float DISTANCE_OFFSET = 10f;
    private const float SCALE_OFFSET = 10f;

    private Piece piece;
    private int index;
    private Bench bench;

    public Piece GetPiece()
    {
        return piece;
    }

    public void SetPiece(Piece piece)
    {
        this.piece = piece;
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetBench(Bench bench)
    {
        this.bench = bench;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale /= SCALE_OFFSET; // update to world scale

        DragEventManager.Instance.draggedPiece = piece;
    }

    // Moves dragged item to mouse position.
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = DISTANCE_OFFSET;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    // Returns dragged item to bench.
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragEventManager.Instance.isPieceDropped)
        {
            Destroy(gameObject);
            bench.RemoveItem(index);
            DragEventManager.Instance.isPieceDropped = false;
        }
        else
        {
            transform.localScale *= SCALE_OFFSET; // update to ui scale
            transform.localPosition = Vector3.zero;
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }
}
