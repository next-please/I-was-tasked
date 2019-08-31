using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Droppable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public virtual void OnBeginDrag(PointerEventData eventData) { }
    public virtual void OnDrag(PointerEventData eventData) { }
    public virtual void OnEndDrag(PointerEventData eventData) { }

    protected Tile GetTileHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            TileView tileView = hit.collider.gameObject.GetComponent<TileView>();
            return tileView == null ? null : tileView.tile;
        }
        return null;
    }

    protected bool IsDropSuccess(Tile tileHit)
    {
        return tileHit != null && !tileHit.IsOccupied();
    }
}
