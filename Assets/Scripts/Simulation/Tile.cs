using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	public Piece occupant;
	public bool isLocked;
	public int row, col;

	public Tile(int row, int col)
	{
		occupant = null;
		isLocked = false;
		this.row = row;
		this.row = col;
	}
}
