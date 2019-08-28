using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float Z_OFFSET = 10f; // distance of the plane from the camera
    private Piece piece;
    private int index;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        transform.localScale /= Z_OFFSET; // update to world scale

        EventManager.Instance.draggedPiece = piece;
    }

    // Moves dragged item to mouse position.
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = Z_OFFSET;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    // Returns dragged item to bench.
    // TODO: Remove item from bench instead?
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale *= Z_OFFSET; // update to ui scale
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Collider>().enabled = true;

        EventManager.Instance.draggedPiece = null;
    }
}
