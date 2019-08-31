using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AddPieceToBoardEvent : GameEvent
{
    public Piece piece;
    public int row;
    public int col;
}

public class RemovePieceFromBoardEvent : GameEvent
{
    public Piece piece;
}

public class Board
{
    private Tile[][] tiles;
    private List<Piece> piecesOnBoard;
    private List<Piece> activePiecesOnBoard;
    private int numRows;
    private int numCols;

    public Board(int numRows, int numCols)
    {
        SetNumRows(numRows);
        SetNumCols(numCols);
        InitialiseGrid();
    }

    private void InitialiseGrid()
    {
        piecesOnBoard = new List<Piece>();
        activePiecesOnBoard = new List<Piece>();
        tiles = new Tile[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            tiles[i] = new Tile[numCols];
            for (int j = 0; j < numCols; j++) {
                tiles[i][j] = new Tile(i, j);
            }
        }
    }

    public void AddPieceToBoard(Piece piece, int i, int j)
    {
        piecesOnBoard.Add(piece);
        activePiecesOnBoard.Add(piece);
        MovePieceToTile(piece, tiles[i][j]);
        piece.SetInitialTile(tiles[i][j]);
        EventManager.Instance.Raise(new AddPieceToBoardEvent { piece = piece, row = i, col = j });
    }

    public void ResetBoard()
    {
        foreach (Piece piece in this.GetPiecesOnBoard())
        {
            if (!activePiecesOnBoard.Contains(piece))
            {
                activePiecesOnBoard.Add(piece);
            }
            piece.Reset();
            MovePieceToTile(piece, piece.GetInitialTile());
        }
    }

    public void RemoveAllPiecesFromBoard()
    {
        var pieces = this.GetPiecesOnBoard().ToArray();
        foreach (Piece piece in pieces)
        {
            RemovePieceFromBoard(piece);
        }
    }

    public void RemoveEnemies()
    {
        // must make a copy before removing enemies
        var enemyPieces = this.GetPiecesOnBoard().Where(piece => piece.IsEnemy()).ToArray();
        foreach (Piece enemy in enemyPieces)
        {
            RemovePieceFromBoard(enemy);
        }
    }

    public Tile GetTile(int row, int col)
    {
        return tiles[row][col];
    }

    public List<Piece> GetPiecesOnBoard()
    {
        return piecesOnBoard;
    }

    public List<Piece> GetActivePiecesOnBoard()
    {
        return activePiecesOnBoard;
    }

    public int GetNumRows()
    {
        return numRows;
    }

    public int GetNumCols()
    {
        return numCols;
    }

    public void MovePieceToTile(Piece piece, Tile nextTile)
    {
        Tile previousTile = piece.GetCurrentTile();
        if (previousTile != null)
        {
            previousTile.SetOccupant(null);
            previousTile.SetLocker(null);
        }
        piece.SetCurrentTile(nextTile);
        piece.SetLockedTile(null);
        nextTile.SetOccupant(piece);

        if (previousTile != null)
        {
            Debug.Log(piece.GetName() + " has moved from " + previousTile.ToString() + " to " + nextTile.ToString() + ".");
        }
        else
        {
            Debug.Log(piece.GetName() + " has been placed at " + nextTile.ToString() + ".");
        }
    }

    public bool CanDeterminePieceLockedTile(Piece piece)
    {
        // Using Modified Breadth First Search (BFS) to find the path and Tile to move to.
        Queue<Tile> queue = new Queue<Tile>();
        bool[][] isVisited = new bool[numRows][];
        Tile[][] predecessors = new Tile[numRows][];
        Piece target = piece.GetTarget();
        Tile currentTile = piece.GetCurrentTile();
        Tile targetTile = target.GetCurrentTile();
        if (target.HasLockedTile()) // Use the Target Tile instead if present.
        {
            targetTile = target.GetLockedTile();
        }

        for (int i = 0; i < numRows; i++)
        {
            isVisited[i] = new bool[numCols];
            predecessors[i] = new Tile[numCols];
            for (int j = 0; j < numCols; j++)
            {
                if (targetTile.Equals(tiles[i][j])) // Mark the Target Tile as Unvisited.
                {
                    isVisited[i][j] = false;
                }
                else // Mark all other occupied and locked Tiles as Visited.
                {
                    isVisited[i][j] = (tiles[i][j].IsOccupied() || tiles[i][j].IsLocked());
                }
            }
        }
        queue.Enqueue(currentTile); // Enqueue our source Tile.

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();
            if (tile.Equals(targetTile)) // Early termination.
            {
                break;
            }

            // Enqueue all surrounding tiles that are Unvisited.
            int tileRow = tile.GetRow();
            int tileCol = tile.GetCol();
            for (int i = tileRow - 1; i <= tileRow + 1; i++)
            {
                for (int j = tileCol - 1; j <= tileCol + 1; j++)
                {
                    if (i < 0 || j < 0 || i >= numRows || j >= numCols) // Boundary Checking.
                    {
                        continue;
                    }

                    if (!isVisited[i][j])
                    {
                        isVisited[i][j] = true;
                        queue.Enqueue(tiles[i][j]);
                        predecessors[i][j] = tile;
                    }
                }
            }
        }

        if (!isVisited[targetTile.GetRow()][targetTile.GetCol()]) // No unobstructed path to the Target.
        {
            piece.SetLockedTile(null);
            return false;
        }

        Tile tileToLock = targetTile;
        while (predecessors[tileToLock.GetRow()][tileToLock.GetCol()] != currentTile)
        {
            tileToLock = predecessors[tileToLock.GetRow()][tileToLock.GetCol()];
        }

        if (tileToLock == targetTile) // Temporary hack for Pathfinding; will be removed later.
        {
            piece.SetLockedTile(null);
            return true;
        }

        tileToLock.SetLocker(piece);
        piece.SetLockedTile(tileToLock);
        return true;
    }

    public Piece FindNearestTarget(Piece piece)
    {
        List<Piece> enemyPiecesOnBoard = new List<Piece>();

        // Get all enemy Pieces on the Board.
        foreach (Piece activePiece in this.GetActivePiecesOnBoard())
        {
            if (activePiece.IsEnemy() != piece.IsEnemy())
            {
                enemyPiecesOnBoard.Add(activePiece);
            }
        }

        // Determine the nearest enemy Piece.
        Piece nearestEnemyPiece = null;

        foreach (Piece enemyPiece in enemyPiecesOnBoard)
        {
            if (nearestEnemyPiece == null)
            {
                nearestEnemyPiece = enemyPiece;
                continue;
            }

            Tile nearestTile = nearestEnemyPiece.GetCurrentTile();
            Tile checkTile = enemyPiece.GetCurrentTile();
            if (piece.GetCurrentTile().DistanceToTile(nearestTile) > piece.GetCurrentTile().DistanceToTile(checkTile))
            {
                nearestEnemyPiece = enemyPiece;
            }
        }
        return nearestEnemyPiece;
    }

    public void SetNumRows(int numRows)
    {
        this.numRows = numRows;
    }

    public void SetNumCols(int numCols)
    {
        this.numCols = numCols;
    }

    public void DeactivatePieceOnBoard(Piece piece)
    {
        Tile tile = piece.GetCurrentTile();
        if (tile != null)
        {
            tile.SetOccupant(null);
            tile.SetLocker(null);
        }
        piece.SetTarget(null);
        piece.SetCurrentTile(null);
        piece.SetLockedTile(null);
        activePiecesOnBoard.Remove(piece);
    }

    public void RemovePieceFromBoard(Piece piece)
    {
        DeactivatePieceOnBoard(piece);
        piecesOnBoard.Remove(piece);
        EventManager.Instance.Raise(new RemovePieceFromBoardEvent{ piece = piece });
    }
}
