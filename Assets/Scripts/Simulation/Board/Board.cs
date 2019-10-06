using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriorityQueue;

public class AddInteractionToProcessEvent : GameEvent
{
    public Interaction interaction;
}

public class Board
{
    private Tile[][] tiles;
    private List<Piece> piecesOnBoard;
    private List<Piece> activePiecesOnBoard;
    private Queue<Interaction> interactionsToProcess;
    private int numRows;
    private int numCols;
    private Player owner;
    private System.Random rngesus;

    // Sort the Pieces by their positions on the Board. Let The rightmost piece be first.
    class PieceSort : IComparer<Piece> {
        public int Compare(Piece x, Piece y)
        {
            Tile tileX = x.GetCurrentTile();
            Tile tileY = y.GetCurrentTile();

            // The Rightmost Piece.
            if (tileX.GetCol() > tileY.GetCol())
            {
                return -1;
            }
            else if (tileX.GetCol() < tileY.GetCol())
            {
                return 1;
            }
            return 0;
        }
    }

    public Board(int numRows, int numCols, Player owner, int seed)
    {
        SetNumRows(numRows);
        SetNumCols(numCols);
        InitialiseGrid();
        this.owner = owner;
        rngesus = new System.Random(seed);
        Debug.Log("A RNGesus with the seed " + seed + " has been created for Player #" + (int) owner + ".");
        interactionsToProcess = new Queue<Interaction>();
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
                tiles[i][j] = new Tile(i, j, this);
            }
        }
    }

    public void MovePieceToTile(Piece piece, Tile nextTile)
    {
        Tile previousTile = piece.GetCurrentTile();
        if (previousTile != null)
        {
            previousTile.SetOccupant(null);
            previousTile.SetLocker(null);
        }
        nextTile.SetLocker(null);
        nextTile.SetOccupant(piece);
        piece.SetCurrentTile(nextTile);
        piece.SetLockedTile(null);

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
        Piece target = piece.GetTarget();
        Tile currentTile = piece.GetCurrentTile();
        Tile targetTile = target.GetCurrentTile();
        if (target.HasLockedTile()) // Use the Target Tile instead if present.
        {
            targetTile = target.GetLockedTile();
        }

        // Using Modified Breadth First Search (BFS) to find the path and Tile to move to.
        //List<Tile> queue = new List<Tile>();
        SimplePriorityQueue<Tile> queue = new SimplePriorityQueue<Tile>();
        bool[][] isVisited = new bool[numRows][];
        Tile[][] predecessors = new Tile[numRows][];

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
        queue.Enqueue(currentTile, 0); // Enqueue our source Tile.

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();

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
                        queue.Enqueue(tiles[i][j], tiles[i][j].ManhattanDistanceToTile(targetTile));
                        predecessors[i][j] = tile;

                        if (tile.Equals(targetTile)) // Early termination.
                        {
                            break;
                        }
                    }
                }
            }
        }

        // No unobstructed path to the Target; attempt to naively move closer to it.
        if (!isVisited[targetTile.GetRow()][targetTile.GetCol()])
        {
            int rowDifference = targetTile.GetRow() - currentTile.GetRow();
            if (rowDifference != 0)
            {
                rowDifference /= Math.Abs(rowDifference);
            }

            int colDifference = targetTile.GetCol() - currentTile.GetCol();
            if (colDifference != 0)
            {
                colDifference /= Math.Abs(colDifference);
            }

            Tile naiveTileToLock = tiles[currentTile.GetRow() + rowDifference][currentTile.GetCol() + colDifference];
            if (!naiveTileToLock.IsLocked() && !naiveTileToLock.IsOccupied())
            {
                naiveTileToLock.SetLocker(piece);
                piece.SetLockedTile(naiveTileToLock);
                return true;
            }

            naiveTileToLock = tiles[currentTile.GetRow()][currentTile.GetCol() + colDifference];
            if (!naiveTileToLock.IsLocked() && !naiveTileToLock.IsOccupied())
            {
                naiveTileToLock.SetLocker(piece);
                piece.SetLockedTile(naiveTileToLock);
                return true;
            }

            naiveTileToLock = tiles[currentTile.GetRow() + rowDifference][currentTile.GetCol()];
            if (!naiveTileToLock.IsLocked() && !naiveTileToLock.IsOccupied())
            {
                naiveTileToLock.SetLocker(piece);
                piece.SetLockedTile(naiveTileToLock);
                return true;
            }

            piece.SetLockedTile(null);
            return false;
        }

        // Backtracking to find the Tile we should move the Piece to.
        Tile tileToLock = targetTile;
        while (predecessors[tileToLock.GetRow()][tileToLock.GetCol()] != currentTile)
        {
            tileToLock = predecessors[tileToLock.GetRow()][tileToLock.GetCol()];
        }

        tileToLock.SetLocker(piece);
        piece.SetLockedTile(tileToLock);
        return true;
    }

    public Piece FindNearestTarget(Piece piece)
    {
        List<Piece> enemyPiecesOnBoard;
        if (!piece.IsEnemy())
        {
            enemyPiecesOnBoard = GetActiveEnemiesOnBoard();
        }
        else
        {
            enemyPiecesOnBoard = GetActiveFriendliesOnBoard();
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

    public Piece FindFarthestTarget(Piece piece)
    {
        List<Piece> enemyPiecesOnBoard;
        if (!piece.IsEnemy())
        {
            enemyPiecesOnBoard = GetActiveEnemiesOnBoard();
        }
        else
        {
            enemyPiecesOnBoard = GetActiveFriendliesOnBoard();
        }

        // Determine the farthest enemy Piece.
        Piece farthestEnemyPiece = null;

        foreach (Piece enemyPiece in enemyPiecesOnBoard)
        {
            if (farthestEnemyPiece == null)
            {
                farthestEnemyPiece = enemyPiece;
                continue;
            }

            Tile farthestTile = farthestEnemyPiece.GetCurrentTile();
            Tile checkTile = enemyPiece.GetCurrentTile();
            if (piece.GetCurrentTile().DistanceToTile(farthestTile) > piece.GetCurrentTile().DistanceToTile(checkTile))
            {
                farthestEnemyPiece = enemyPiece;
            }
        }
        return farthestEnemyPiece;
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

        Tile lockedTile = piece.GetLockedTile();
        if (lockedTile != null)
        {
            lockedTile.SetLocker(null);
        }
        piece.SetLockedTile(null);

        activePiecesOnBoard.Remove(piece);
        activePiecesOnBoard.Sort(new PieceSort());
    }

    public void RemovePieceFromBoard(Piece piece)
    {
        DeactivatePieceOnBoard(piece);
        piecesOnBoard.Remove(piece);
        piecesOnBoard.Sort(new PieceSort());
    }

    public void ClearInteractionsToProcess()
    {
        while (interactionsToProcess.Count > 0)
        {
            Interaction interaction = interactionsToProcess.Dequeue();
            interaction.CleanUpInteraction();
        }
    }

    public void AddPieceToBoard(Piece piece, int i, int j)
    {
        piecesOnBoard.Add(piece);
        activePiecesOnBoard.Add(piece);
        MovePieceToTile(piece, tiles[i][j]);
        piece.SetInitialTile(tiles[i][j]);
        piece.SetCurrentTile(tiles[i][j]);
        activePiecesOnBoard.Sort(new PieceSort());
        piecesOnBoard.Sort(new PieceSort());
    }

    public void AddInteractionToProcess(Interaction interaction)
    {
        interactionsToProcess.Enqueue(interaction);
        EventManager.Instance.Raise(new AddInteractionToProcessEvent
        {
            interaction = interaction
        });
    }

    public Tile GetTile(int row, int col)
    {
        return tiles[row][col];
    }

    public Piece GetPiece(Piece piece)
    {
        return GetPiecesOnBoard().Find(actualPiece => actualPiece == piece);
    }

    public List<Piece> GetPiecesOnBoard()
    {
        return piecesOnBoard;
    }

    public List<Piece> GetActivePiecesOnBoard()
    {
        return activePiecesOnBoard;
    }

    public List<Piece> GetActiveEnemiesOnBoard()
    {
        return activePiecesOnBoard.FindAll(p => p.IsEnemy());
    }

    public List<Piece> GetActiveEnemiesWithinRadiusOfTile(Tile tile, int radius)
    {
        return activePiecesOnBoard.FindAll(p => p.IsEnemy() && (Math.Abs(p.GetCurrentTile().GetRow() - tile.GetRow()) <= radius) && (Math.Abs(p.GetCurrentTile().GetCol() - tile.GetCol()) <= radius));
    }

    public List<Piece> GetActiveFriendliesOnBoard()
    {
        return activePiecesOnBoard.FindAll(p => !p.IsEnemy());
    }

    public Queue<Interaction> GetInteractionsToProcess()
    {
        return interactionsToProcess;
    }

    public int GetNumRows()
    {
        return numRows;
    }

    public int GetNumCols()
    {
        return numCols;
    }

    public System.Random GetRNGesus()
    {
        return rngesus;
    }

    public Player GetOwner()
    {
        return owner;
    }

    public void SetNumRows(int numRows)
    {
        this.numRows = numRows;
    }

    public void SetNumCols(int numCols)
    {
        this.numCols = numCols;
    }
}
