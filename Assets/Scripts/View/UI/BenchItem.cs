using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Piece piece;

    public void SetPiece(Piece piece)
    {
        this.piece = piece;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EventManager.Instance.draggedPiece = piece;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // Moves dragged item to mouse position.
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        transform.localScale = new Vector3(5, 5, 5);
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    // Returns dragged item to bench.
    // TODO: Remove item from bench instead?
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = new Vector3(50, 50, 50);
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Collider>().enabled = false;

        EventManager.Instance.draggedPiece = null;
    }
}
