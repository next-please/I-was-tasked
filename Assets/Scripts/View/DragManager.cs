using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    public ArrangementManager arrangementManager;
    public TransactionManager transactionManager;
    public RoomManager roomManager;
    public Canvas trashCan;
    public GameObject Highlight;

    void OnEnable()
    {
        EventManager.Instance.AddListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.AddListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.AddListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.AddListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.AddListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.AddListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
        EventManager.Instance.AddListener<ShowTrashCanEvent>(OnShowTrashCanEvent);
        EventManager.Instance.AddListener<DragOverTileEvent>(OnDragOverTile);
        EventManager.Instance.AddListener<DragEndEvent>(OnDragEnd);

        trashCan.enabled = false;
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.RemoveListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.RemoveListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.RemoveListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.RemoveListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.RemoveListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
        EventManager.Instance.RemoveListener<ShowTrashCanEvent>(OnShowTrashCanEvent);
        EventManager.Instance.RemoveListener<DragOverTileEvent>(OnDragOverTile);
        EventManager.Instance.RemoveListener<DragEndEvent>(OnDragEnd);
    }

    void Start()
    {
        Player player = RoomManager.GetLocalPlayer();
        Highlight.transform.Rotate(Vector3.up, 45 * (int)player, Space.World);
    }


    void OnShowTrashCanEvent(ShowTrashCanEvent e)
    {
        if (!roomManager.IsTutorial)
        {
            trashCan.enabled = e.showTrashCan;
        }
    }

    void OnMovePieceOnBoard(MoveOnBoardEvent e)
    {
        arrangementManager.TryMovePieceOnBoard(RoomManager.GetLocalPlayer(), e.piece, e.tile);
    }

    void OnMoveFromBoardToBench(MoveFromBoardToBenchEvent e)
    {
        if (e.slotIndex < 0)
        {
            return;
        }
        arrangementManager.TryMoveBoardToBench(RoomManager.GetLocalPlayer(), e.piece, e.slotIndex);
    }

    void OnMoveFromBenchToBoard(MoveFromBenchToBoardEvent e)
    {
        if (e.tile.IsOccupied() || e.tile.IsEnemyTile())
        {
            return;
        }
        arrangementManager.TryMoveBenchToBoard(RoomManager.GetLocalPlayer(), e.piece, e.tile);
    }

    void OnMoveOnBench(MoveOnBenchEvent e)
    {
        arrangementManager.TryMovePieceOnBench(RoomManager.GetLocalPlayer(), e.piece, e.slotIndex);
    }

    void OnTrashPieceOnBoardEvent(TrashPieceOnBoardEvent e)
    {
        transactionManager.TrySellBoardPiece(RoomManager.GetLocalPlayer(), e.piece);
    }

    void OnTrashPieceOnBenchEvent(TrashPieceOnBenchEvent e)
    {
        transactionManager.TrySellBenchPiece(RoomManager.GetLocalPlayer(), e.piece);
    }

    void OnDragOverTile(DragOverTileEvent tileEvent)
    {
        HitTarget target = tileEvent.hitTarget;
        if (target == HitTarget.Tile)
        {
            Tile tile = tileEvent.targetObject.GetComponent<TileView>().GetTile();
            if (tile == null)
            {
                Highlight.SetActive(false);
                return;
            }
            Vector3 tilePosition = ViewManager.CalculateTileWorldPosition(tile);
            tilePosition.y = 0.75f;
            Highlight.transform.position = tilePosition;
            bool isValidTile = !tile.IsEnemyTile() &&
                tile.GetBoard().GetOwner() == RoomManager.GetLocalPlayer();
            Highlight.SetActive(isValidTile);
        }
        else if (target == HitTarget.BenchSlot)
        {
            Vector3 position = tileEvent.targetObject.transform.position;
            BenchSlot benchSlot = tileEvent.targetObject.GetComponent<BenchSlot>();
            position.y = 0.75f;
            Highlight.transform.position = position;
            bool isValidBenchSlot = benchSlot.Owner == RoomManager.GetLocalPlayer();
            Highlight.SetActive(isValidBenchSlot);
        }
        else
        {
            Highlight.SetActive(false);
        }
    }

    void OnDragEnd(DragEndEvent e)
    {
        Highlight.SetActive(false);
    }
}
