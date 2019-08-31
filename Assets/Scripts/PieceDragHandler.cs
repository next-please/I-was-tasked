using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PieceDragHandler : Droppable
{
    private float zPos;
    private Vector3 originalPos;
    private Piece piece;

    void Start()
    {
        piece = gameObject.GetComponent<PieceView>().piece;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.position;
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;

        EventManager.Instance.Raise(new PieceDragEvent { piece = piece });
        SetDraggedState();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Tile tileHit = GetTileHit();

        if (tileHit == null | tileHit.IsOccupied())
        {
            SetBoardState();
            transform.position = originalPos;
        }
        else
        {
            EventManager.Instance.Raise(new PieceDropEvent { tile = tileHit });
            Destroy(gameObject);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPosition();
    }

    private void SetBoardState()
    {
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void SetDraggedState()
    {
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zPos;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
