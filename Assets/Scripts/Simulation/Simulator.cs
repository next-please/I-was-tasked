using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimulationEndedEvent : GameEvent
{

}

public class Simulator : Tickable
{
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
            EventManager.Instance.Raise(new SimulationEndedEvent {});
        }
    }

    public void SetGameBoard(Board board)
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
