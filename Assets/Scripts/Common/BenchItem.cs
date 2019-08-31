using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BenchItem : Droppable
{
    private readonly float distanceOffset = 10f;
    private readonly float scaleOffset = 10f;

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

    public override void OnEndDrag(PointerEventData eventData)
    {
        Tile tileHit = GetTileHit();

        if (IsDropSuccess(tileHit))
        {
            EventManager.Instance.Raise(new PieceDropEvent { tile = tileHit });
            Destroy();
        }
        else
        {
            ReturnToBench();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
        EventManager.Instance.Raise(new BenchItemRemovedEvent { removedItem = this });
    }

    private void ReturnToBench()
    {
        transform.localScale *= scaleOffset; // update to ui scale
        transform.localPosition = Vector3.zero;
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
