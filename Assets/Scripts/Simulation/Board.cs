using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	public Tile[][] tiles;
	public Dictionary<Piece, Tile> pieceToTile;
	public int rows;
	public int cols;

	public Board(int rows, int cols)
	{
		this.rows = rows;
		this.cols = cols;
		InitialiseGrid();
		pieceToTile = new Dictionary<Piece, Tile>();
	}

	private void InitialiseGrid()
	{
		tiles = new Tile[rows][];
		for (int i = 0; i < rows; i++)
		{
			tiles[i] = new Tile[cols];
			for (int j = 0; j < cols; j++) {
				tiles[i][j] = new Tile(i, j);
			}
		}
	}

	public void SetPieceAtTile(Piece piece, Tile tile)
	{
		RemovePieceFromBoard(piece);
		pieceToTile.Add(piece, tile);
		tile.occupant = piece;
	}

	public void RemovePieceFromBoard(Piece piece)
	{
		if (HasPieceOnBoard(piece))
		{
			Tile previousTile = GetCurrentTileOfPiece(piece);
			previousTile.occupant = null;
			pieceToTile.Remove(piece);
		}
	}

	public bool HasPieceOnBoard(Piece piece)
	{
		return pieceToTile.ContainsKey(piece);
	}

	public Tile GetCurrentTileOfPiece(Piece piece)
	{
		return pieceToTile[piece];
	}

	public List<Piece> GetPiecesOnBoard()
	{
		List<Piece> piecesOnBoard = new List<Piece>();
		foreach (Piece piece in pieceToTile.Keys)
		{
			piecesOnBoard.Add(piece);
		}
		return piecesOnBoard;
	}
}
