using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PieceDragHandler : Droppable
{
    // Fix z to avoid piece clipping into board, have to adjust later (also consider adjusting scale)
    public readonly float zPosOnDrag = 10f;
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
        // zPos = Camera.main.WorldToScreenPoint(transform.position).z;

        EventManager.Instance.Raise(new PieceDragEvent { piece = piece });
        SetDraggedState();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Tile tileHit = GetTileHit();

        if (IsDropSuccess(tileHit))
        {
            EventManager.Instance.Raise(new PieceDropEvent { tile = tileHit });
            Destroy(gameObject);
        }
        else
        {
            // return to board
            SetBoardState();
            transform.position = originalPos;
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
        // mousePosition.z = zPos;
        mousePosition.z = zPosOnDrag;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
