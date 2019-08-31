using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Piece draggedPiece;

    public Simulator sim;
    public PlayerInventory playerInventory;
    EnemyGenerator enemyGenerator = new EnemyGenerator();
    void OnEnable()
    {
        EventManager.Instance.AddListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.AddListener<PieceDragEvent>(OnPieceDrag);
        EventManager.Instance.AddListener<PieceDropEvent>(OnPieceDrop);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<EnterPhaseEvent>(OnEnterPhase);
        EventManager.Instance.RemoveListener<PieceDragEvent>(OnPieceDrag);
        EventManager.Instance.RemoveListener<PieceDropEvent>(OnPieceDrop);
    }

    void OnEnterPhase(EnterPhaseEvent e)
    {
        if (e.phase == Phase.Initialization)
        {
            CreateBoard();
        }

        if (e.phase == Phase.PreCombat)
        {
            SummonEnemiesAndAllies(e.round);
        }

        if (e.phase == Phase.Combat)
        {
            StartSim();
        }
    }

    void CreateBoard()
    {
        sim.CreateBoard(8, 8);
    }

    void StartSim()
    {
        sim.StartSim();
    }

    void SummonEnemiesAndAllies(int currentRound)
    {
        sim.gameBoard.ResetBoard();
        sim.gameBoard.RemoveAllPiecesFromBoard(); // for now
        // sim.gameBoard.RemoveEnemies();
        ArrayList enemyPieces = enemyGenerator.generateEnemies(currentRound);
        for (int i = 0; i < enemyPieces.Count; i++)
        {
            sim.AddPieceToBoard((Piece)enemyPieces[i], 7 - (i / 8), 7 - (i % 8));
        }

        // for debug
        int x = 0;
        foreach (Piece piece in playerInventory.GetGarrison())
        {
            sim.AddPieceToBoard(piece, x / 8, x % 8);
            x++;
        }
    }

    public void OnPieceDrag(PieceDragEvent e)
    {
        draggedPiece = e.piece;
    }

    public void OnPieceDrop(PieceDropEvent e)
    {
        if (e.tile == null || e.tile.IsOccupied())
        {
            return;
        }

        sim.AddPieceToBoard(draggedPiece, e.tile.GetRow(), e.tile.GetCol());
    }
}

public class PieceDragEvent : GameEvent
{
    public Piece piece;
}

public class PieceDropEvent : GameEvent
{
    public Tile tile;
}
