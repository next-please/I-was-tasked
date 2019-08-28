using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Simulator sim;
    public PlayerInventory playerInventory;
    EnemyGenerator enemyGenerator = new EnemyGenerator();
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
            sim.AddPieceToBoard((Piece)enemyPieces[i], 7- (i / 8), 7 - (i % 8));
        }

        // for debug
        int x = 0;
        foreach (Piece piece in playerInventory.GetGarrison())
        {
            sim.AddPieceToBoard(piece, x / 8, x % 8);
            x++;
        }
    }
}
