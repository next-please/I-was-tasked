using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;


[Serializable]
public class Tile : ISerializable
{
    #region Serializable Fields
    private int row;
    private int col;
    #endregion

    private Piece occupant;
    private Piece locker;

    public Tile(int row, int col)
    {
        SetOccupant(null);
        SetLocker(null);
        SetRow(row);
        SetCol(col);
    }

    public Tile(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        row = (int) info.GetValue("row", typeof(int));
        col = (int) info.GetValue("col", typeof(int));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // In Order of Declaration
        info.AddValue("row", row, typeof(int));
        info.AddValue("col", col, typeof(int));
    }

    public int DistanceToTile(Tile tile)
    {
        int rowDifference = Math.Abs(this.GetRow() - tile.GetRow());
        int colDifference = Math.Abs(this.GetCol() - tile.GetCol());
        int distance = rowDifference + colDifference;
        if (rowDifference == colDifference) // The tiles are diagonal to each other.
        {
            distance /= 2;
        }
        return distance; // Manhattan Distance.
    }

    public int ManhattanDistanceToTile(Tile tile)
    {
        int rowDifference = Math.Abs(this.GetRow() - tile.GetRow());
        int colDifference = Math.Abs(this.GetCol() - tile.GetCol());
        int distance = rowDifference + colDifference;
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
