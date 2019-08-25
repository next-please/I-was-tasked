using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	private Piece occupant;
	private Piece locker;
	private int row;
	private int col;

	public Tile(int row, int col)
	{
		SetOccupant(null);
		SetLocker(null);
		SetRow(row);
		SetCol(col);
	}

	public int DistanceToTile(Tile tile)
	{
		int rowDifference = Math.Abs(this.GetRow() - tile.GetRow());
		int colDifference = Math.Abs(this.GetCol() - tile.GetCol());
		int distance = (int) Math.Floor(Math.Sqrt(Math.Pow(rowDifference, 2) + Math.Pow(colDifference, 2)));
		return distance;
	}

	public Piece GetOccupant()
	{
		return occupant;
	}

	public Piece GetLocker()
	{
		return locker;
	}

	public bool IsOccupied()
	{
		return (occupant != null);
	}

	public bool IsLocked()
	{
		return (locker != null);
	}

	public int GetRow()
	{
		return row;
	}

	public int GetCol()
	{
		return col;
	}

	public void SetOccupant(Piece occupant)
	{
		this.occupant = occupant;
	}

	public void SetLocker(Piece locker)
	{
		this.locker = locker;
	}

	public void SetRow(int row)
	{
		this.row = row;
	}

	public void SetCol(int col)
	{
		this.col = col;
	}

	public override string ToString()
	{
		return "(" + row + "," + col + ")";
	}
}
