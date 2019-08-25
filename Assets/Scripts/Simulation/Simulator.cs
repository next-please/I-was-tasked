using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : Tickable
{
    private Board gameBoard;
    private bool isResolved;

    private void Awake()
    {
        MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, true);
        MeleePiece junkai_enemy = new MeleePiece("Jun the Supreme Kai", 100, 2, true);
        MeleePiece jolyn_player = new MeleePiece("Jo Jo Lyn", 100, 3, false);
        MeleePiece nicholas_player = new MeleePiece("Nick Pepega Chua", 100, 4, false);
        gameBoard = new Board(8, 8);
        gameBoard.AddPieceToBoard(lewis_enemy, 7, 7);
        gameBoard.AddPieceToBoard(junkai_enemy, 4, 4);
        gameBoard.AddPieceToBoard(jolyn_player, 1, 2);
        gameBoard.AddPieceToBoard(nicholas_player, 0, 0);
        isResolved = false;
    }

    void CheckResolved()
    {
        List<Piece> piecesOnBoard = gameBoard.GetActivePiecesOnBoard();
        int numEnemies = piecesOnBoard.Where(piece => piece.IsEnemy()).Count();
        int numFriends = piecesOnBoard.Where(piece => !piece.IsEnemy()).Count();
        if (numEnemies == 0 || numFriends == 0)
            isResolved = true;
    }

    public override void Tick(long tick)
    {
        if (isResolved)
        {
            Debug.Log("resolved");
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
