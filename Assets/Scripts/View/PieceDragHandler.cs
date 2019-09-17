using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveFromBoardToBenchEvent : GameEvent
{
    public Piece piece;
    public int slotIndex;
}

public class MoveOnBoardEvent : GameEvent
{
    public Piece piece;
    public Tile tile;
}

public class TrashPieceOnBoardEvent : GameEvent
{
    public Piece piece;
}

public class PieceDragHandler : InteractablePiece
{
    public readonly float zPosOnDrag = 10f;
    private Vector3 originalPos;
    private Animator animator;
    private bool IsDragAllowed;

    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Market)
        {
            IsDragAllowed = true;
        }
        else
        {
            IsDragAllowed = false;
        }
    }

    void Start()
    {
        PieceView pieceView = gameObject.GetComponent<PieceView>();
        animator = pieceView.animator;
        piece = pieceView.piece;
        if (piece.IsEnemy())
            Destroy(this);

        // when this piece is spawned, we assume its during market phase
        // and thus it should be draggable until the next phase
        IsDragAllowed = true;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragAllowed)
        {
            originalPos = transform.position;
            SetDraggedState();
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPosition();
    }

    public override void OnBenchDrop(BenchSlot slot)
    {
        if (slot.isOccupied)
        {
            OnEmptyDrop();
            return;
        }

        EventManager.Instance.Raise(new MoveFromBoardToBenchEvent
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
            OnEmptyDrop();
            return;
        }
        EventManager.Instance.Raise(new MoveOnBoardEvent { piece = piece, tile = tile });
        SetBoardState();
    }

    public override void OnTrashDrop()
    {
        EventManager.Instance.Raise(new TrashPieceOnBoardEvent { piece = piece });
    }

    public override void OnEmptyDrop()
    {
        SetBoardState();
        transform.position = originalPos;
    }

    private void SetBoardState()
    {
        animator.Play("Idle");
        // gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        // gameObject.GetComponent<Collider>().enabled = true;
    }

    private void SetDraggedState()
    {
        animator.Play("Walk");
        // gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // gameObject.GetComponent<Collider>().enabled = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zPosOnDrag;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
