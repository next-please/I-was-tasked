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
    private Vector3 originalPos;
    private Animator animator;

    bool inCombat = false;

    void OnEnable()
    {
        EventManager.Instance.AddListener<PieceMoveEvent>(OnPieceMove);
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<PieceMoveEvent>(OnPieceMove);
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
    }

    void Start()
    {
        PieceView pieceView = gameObject.GetComponent<PieceView>();
        animator = pieceView.animator;
        piece = pieceView.piece;
        if (piece.IsEnemy())
            Destroy(this);
        originalPos = transform.position;
    }

    public void OnEnterPhase(EnterPhaseEvent e)
    {
        inCombat = e.phase == Phase.Combat;
        if (inCombat)
        {
            OnEmptyDrop();
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (IsDragAllowed())
        {
            transform.position = GetMouseWorldPosition();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragAllowed())
        {
            originalPos = transform.position;
            SetDraggedState();
            EventManager.Instance.Raise(new ShowTrashCanEvent { showTrashCan = true });
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public override void OnBenchDrop(BenchSlot slot)
    {
        if (slot.isOccupied || inCombat)
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
        if (tile.IsOccupied() || !tile.IsEnemyTile())
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
    }

    private void SetDraggedState()
    {
        animator.Play("Walk");
    }

    private void OnPieceMove(PieceMoveEvent e)
    {
        if (e.piece == piece)
        {
            originalPos = ViewManager.CalculateTileWorldPosition(e.tile);
            originalPos.y = 0.5f;
        }
    }

    private bool IsDragAllowed()
    {
        Phase currentPhase = PhaseManager.GetCurrentPhase();
        return (currentPhase == Phase.Market || currentPhase == Phase.PreCombat) && CameraController.IsViewingOwnBoard();
    }
}
