using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HitTarget
{
    Empty = 0,
    Tile = 1,
    BenchSlot = 2,
    Trash = 3
}

public abstract class Droppable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected GameObject targetObject;

    public virtual void OnBeginDrag(PointerEventData eventData) { }
    public virtual void OnDrag(PointerEventData eventData) { }
    public virtual void OnBenchDrop(BenchSlot slot) { }
    public virtual void OnTileDrop(Tile tile) { }
    public virtual void OnTrashDrop() { }
    public virtual void OnEmptyDrop() { }

    protected HitTarget GetHitTarget()
    {
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        foreach (RaycastResult h in hits)
        {
            if (h.gameObject.GetComponent<TileView>() != null)
            {
                targetObject = h.gameObject;
                return HitTarget.Tile;
            }
            if (h.gameObject.GetComponent<BenchSlot>() != null)
            {
                targetObject = h.gameObject;
                return HitTarget.BenchSlot;
            }
            if (h.gameObject.name == "Trash")
            {
                return HitTarget.Trash;
            }
        }
        targetObject = null;
        return HitTarget.Empty;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HitTarget target = GetHitTarget();
        switch (target)
        {
            case HitTarget.Tile:
                OnTileDrop(targetObject.GetComponent<TileView>().tile);
                break;
            case HitTarget.BenchSlot:
                OnBenchDrop(targetObject.GetComponent<BenchSlot>());
                break;
            case HitTarget.Trash:
                OnTrashDrop();
                break;
            case HitTarget.Empty:
                OnEmptyDrop();
                break;
            default:
                break;
        }
    }
}
