using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : Tickable
{
    public Board gameBoard;
    private bool isResolved = false;

    void CheckResolved()
    {
        List<Piece> piecesOnBoard = gameBoard.GetActivePiecesOnBoard();
        int numEnemies = piecesOnBoard.Where(piece => piece.IsEnemy()).Count();
        int numFriends = piecesOnBoard.Where(piece => !piece.IsEnemy()).Count();
        if (numEnemies == 0 || numFriends == 0)
        {
            isResolved = true;
            Debug.Log("Game has been resolved");
        }
    }

    protected new void Start()
    {
        base.Start();
        isResolved = false;
        if (gameBoard == null)
        {
            Debug.Log("Gameboard not set before Start()!");
            isResolved = true;
        }
    }

    public override void Tick(long tick)
    {
        if (isResolved)
        {
            return;
        }
        CheckResolved();
        List<Piece> piecesOnBoard = gameBoard.GetPiecesOnBoard();
        foreach (Piece currentPiece in piecesOnBoard)
        {
            currentPiece.ProcessAction(gameBoard, tick);
        }
    }
}
