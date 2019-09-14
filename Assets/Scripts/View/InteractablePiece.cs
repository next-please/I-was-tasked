using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HitTarget
{
    Empty,
    Tile,
    BenchSlot,
    Trash
}

public class SelectPieceEvent : GameEvent
{
    public Piece piece;
}

public class DeselectPieceEvent : GameEvent { }

// Handles drag and drop, selection and deselection of piece
public abstract class InteractablePiece :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerDownHandler,
    ISelectHandler,
    IDeselectHandler
{
    private readonly float zPosOnDrag = 10f;

    public Piece piece;
    protected GameObject targetObject;

    public virtual void OnBeginDrag(PointerEventData eventData) { }
    public virtual void OnDrag(PointerEventData eventData) { }
    public virtual void OnBenchDrop(BenchSlot slot) { }
    public virtual void OnTileDrop(Tile tile) { }
    public virtual void OnTrashDrop() { }
    public virtual void OnEmptyDrop() { }

    public void OnEndDrag(PointerEventData eventData)
    {
        eventData.selectedObject = null;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnSelect(BaseEventData eventData)
    {
        EventManager.Instance.Raise(new SelectPieceEvent { piece = piece });
    }

    public void OnDeselect(BaseEventData eventData)
    {
        EventManager.Instance.Raise(new DeselectPieceEvent { });
    }

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

    protected Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zPosOnDrag;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
