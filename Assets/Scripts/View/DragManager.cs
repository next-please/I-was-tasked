using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public ArrangementManager arrangementManager;
    public TransactionManager transactionManager;
    public Player player;
    private Piece draggedPiece;
    void OnEnable()
    {
        EventManager.Instance.AddListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.AddListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.AddListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.AddListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.AddListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.AddListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<MoveOnBoardEvent>(OnMovePieceOnBoard);
        EventManager.Instance.RemoveListener<MoveFromBoardToBenchEvent>(OnMoveFromBoardToBench);
        EventManager.Instance.RemoveListener<MoveFromBenchToBoardEvent>(OnMoveFromBenchToBoard);
        EventManager.Instance.RemoveListener<MoveOnBenchEvent>(OnMoveOnBench);
        EventManager.Instance.RemoveListener<TrashPieceOnBoardEvent>(OnTrashPieceOnBoardEvent);
        EventManager.Instance.RemoveListener<TrashPieceOnBenchEvent>(OnTrashPieceOnBenchEvent);
    }

    void OnMovePieceOnBoard(MoveOnBoardEvent e)
    {
        arrangementManager.TryMovePieceOnBoard(player, e.piece, e.tile);
    }

    void OnMoveFromBoardToBench(MoveFromBoardToBenchEvent e)
    {
        if (e.slotIndex < 0)
        {
            return;
        }
        Debug.Log("event received");
        arrangementManager.TryMoveBoardToBench(player, e.piece, e.slotIndex);
    }

    void OnMoveFromBenchToBoard(MoveFromBenchToBoardEvent e)
    {
        if (e.tile.IsOccupied())
        {
            return;
        }
        arrangementManager.TryMoveBenchToBoard(player, e.piece, e.tile);
    }

    void OnMoveOnBench(MoveOnBenchEvent e)
    {
        arrangementManager.TryMovePieceOnBench(player, e.piece, e.slotIndex);
    }

    void OnTrashPieceOnBoardEvent(TrashPieceOnBoardEvent e)
    {
        arrangementManager.TryRemovePieceOnBoard(player, e.piece);
    }

    void OnTrashPieceOnBenchEvent(TrashPieceOnBenchEvent e)
    {
        transactionManager.TrySellBenchPiece(player, e.piece);
    }
}
