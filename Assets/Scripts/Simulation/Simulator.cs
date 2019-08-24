using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Tickable
{
	public Board gameBoard;

	private void Awake()
	{
		gameBoard = new Board(8, 8);
		MeleePiece lewis_enemy = new MeleePiece("Lewis the Jesus Koh", 100, 1, 1, true);
		MeleePiece nicholas_player = new MeleePiece("Nick Pepega Chua", 100, 2, 1, false);
		gameBoard.SetPieceAtTile(lewis_enemy, gameBoard.tiles[0][0]);
		gameBoard.SetPieceAtTile(nicholas_player, gameBoard.tiles[7][7]);
	}
	
	public override void Tick(long tick)
	{
		List<Piece> pieces = gameBoard.GetPiecesOnBoard();
		int currentPiece = (int) (tick % pieces.Count);
		Debug.Log("The current piece is " + pieces[currentPiece].name);
	}
}
