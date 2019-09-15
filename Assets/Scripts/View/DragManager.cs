﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class DragManager : MonoBehaviour
{
    public ArrangementManager arrangementManager;
    public TransactionManager transactionManager;
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
        arrangementManager.TryMovePieceOnBoard(RoomManager.LocalPlayer, e.piece, e.tile);
    }

    void OnMoveFromBoardToBench(MoveFromBoardToBenchEvent e)
    {
        if (e.slotIndex < 0)
        {
            return;
        }
        arrangementManager.TryMoveBoardToBench(RoomManager.LocalPlayer, e.piece, e.slotIndex);
    }

    void OnMoveFromBenchToBoard(MoveFromBenchToBoardEvent e)
    {
        if (e.tile.IsOccupied())
        {
            return;
        }
        arrangementManager.TryMoveBenchToBoard(RoomManager.LocalPlayer, e.piece, e.tile);
    }

    void OnMoveOnBench(MoveOnBenchEvent e)
    {
        arrangementManager.TryMovePieceOnBench(RoomManager.LocalPlayer, e.piece, e.slotIndex);
    }

    void OnTrashPieceOnBoardEvent(TrashPieceOnBoardEvent e)
    {

        if (arrangementManager.TryRemovePieceOnBoard(RoomManager.LocalPlayer, e.piece))
        {
            transactionManager.SellPiece(RoomManager.LocalPlayer, e.piece);
        }
    }

    void OnTrashPieceOnBenchEvent(TrashPieceOnBenchEvent e)
    {
        transactionManager.TrySellBenchPiece(RoomManager.LocalPlayer, e.piece);
    }
}
