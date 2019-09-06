using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : Tickable
{
    public PhaseManager phaseManager;
    private Player player;
    private Board gameBoard;
    public bool shouldRun = false;

    void CheckResolved()
    {
        List<Piece> piecesOnBoard = gameBoard.GetActivePiecesOnBoard();
        int numEnemies = piecesOnBoard.Where(piece => piece.IsEnemy()).Count();
        int numFriends = piecesOnBoard.Where(piece => !piece.IsEnemy()).Count();
        if (numEnemies == 0 || numFriends == 0)
        {
            shouldRun = false;
            Debug.Log("Game has been resolved");
            phaseManager.SimulationEnded(player, piecesOnBoard);
        }
    }

    public void SetGameBoard(Board board, Player player = Player.Zero)
    {
        gameBoard = board;
    }

    public void StartSim()
    {
        shouldRun = true;
    }

    public override void Tick(long tick)
    {
        if (!shouldRun)
        {
            return;
        }
        CheckResolved();
        List<Piece> piecesOnBoard = gameBoard.GetPiecesOnBoard();
        foreach (Piece currentPiece in piecesOnBoard)
        {
            currentPiece.ProcessState(gameBoard, tick);
        }
    }
}
