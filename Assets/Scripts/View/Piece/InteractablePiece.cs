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

public class HoverPieceEvent : GameEvent
{
    public Piece piece;
}

// can be tile / bench
public class DragOverTileEvent : GameEvent
{
    public HitTarget hitTarget;
    public GameObject targetObject;
}

public class DragEndEvent : GameEvent
{
}

public class DeselectPieceEvent : GameEvent { }

// Handles drag and drop, selection and deselection of piece
public abstract class InteractablePiece :
    MonoBehaviour,
    IDragHandler,
    IPointerUpHandler,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler
{
    private readonly float zPosOnDrag = 8f;

    public Piece piece;
    public GameObject[] rarities;
    public GameObject rarityParent;
    protected GameObject targetObject;

    public virtual void OnDrag(PointerEventData eventData) { }
    public virtual void OnBenchDrop(BenchSlot slot) { }
    public virtual void OnTileDrop(Tile tile) { }
    public virtual void OnTrashDrop() { }
    public virtual void OnEmptyDrop() { }

    void Update()
    {
        if (!CameraController.IsViewingOwnBoard())
        {
            OnEmptyDrop();
            EventManager.Instance.Raise(new DragEndEvent { });
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        eventData.selectedObject = null;

        HitTarget target = GetHitTarget();
        switch (target)
        {
            case HitTarget.Tile:
                OnTileDrop(targetObject.GetComponent<TileView>().GetTile());
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
        EventManager.Instance.Raise(new PieceHandleEvent { piece = piece, isHeld = false });
        EventManager.Instance.Raise(new DragEndEvent { });
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
        HitTarget target = GetHitTarget();
        EventManager.Instance.Raise(new DragOverTileEvent
        {
            hitTarget = target,
            targetObject = this.targetObject
        });
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
            if (h.gameObject.tag == "Trash")
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
        bool hit = false;
        Vector3 pos = CameraController.GetMousePositionOnBoard(ref hit);
        return hit ? pos : Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Instance.Raise(new HoverPieceEvent { piece = piece });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Instance.Raise(new HoverPieceEvent { piece = null });
    }
}
