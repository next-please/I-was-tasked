using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PieceDragEvent : GameEvent
{
    public Piece piece;
}

public class PieceDropOnBoardEvent : GameEvent
{
    public Tile tile;
}

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

    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPosition();
    }

    public override void OnBenchDrop(BenchSlot slot)
    {
        EventManager.Instance.Raise(new AddPieceToBenchEvent
        {
            piece = piece,
            slotIndex = slot.index
        });
        Destroy(gameObject);
    }

    public override void OnTileDrop(Tile tile)
    {
        if (tile.IsOccupied())
        {
            return;
        }
        EventManager.Instance.Raise(new PieceDropOnBoardEvent { tile = tile });
        Destroy(gameObject);
    }

    public override void OnTrashDrop()
    {
        // Todo: remove piece from board properly
        EventManager.Instance.Raise(new RemovePieceFromBoardEvent { piece = piece });
    }

    public override void OnEmptyDrop()
    {
        SetBoardState();
        transform.position = originalPos;
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
