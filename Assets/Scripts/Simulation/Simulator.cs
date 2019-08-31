using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimulationEndedEvent : GameEvent
{

}

public class Simulator : Tickable
{
    public ViewManager viewManager;
    public Board gameBoard;
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

    public void CreateBoard(int rows, int cols)
    {
        gameBoard = new Board(8, 8);
        viewManager.OnBoardCreated(gameBoard);
    }

    public void AddPieceToBoard(Piece piece, int i, int j)
    {
        gameBoard.AddPieceToBoard(piece, i, j);
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
