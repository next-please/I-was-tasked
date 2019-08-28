using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Piece piece;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPiece(Piece piece)
    {
        this.piece = piece;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        EventManager.Instance.draggedPiece = piece;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        EventManager.Instance.draggedPiece = null;
    }
}
