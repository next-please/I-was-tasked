﻿using System.Collections;
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

public class PieceMoveEvent : GameEvent
{
    public Piece piece;
    public Tile tile;
}

public class BoardManager : MonoBehaviour
{
    public int numPlayers = 1;
    public Simulator[] Simulators;
    public ViewManager viewManager;
    Board[] boards;

    public void CreateBoards()
    {
        boards = new Board[numPlayers];
        for (int i = 0; i < numPlayers; ++i)
        {
            Simulator sim = Simulators[i];
            Board board = new Board(8, 8);
            boards[i] = board;
            sim.SetGameBoard(board);
            viewManager.OnBoardCreated(board);
        }
    }

    public void StartSim()
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Simulator sim = Simulators[i];
            sim.StartSim();
        }
    }

    public Board GetBoard(Player player)
    {
        int index = (int) player;
        return boards[index];
    }

    public void ResetBoards()
    {
        for (int i = 0; i < numPlayers; ++i)
        {
            Player player = (Player) i;
            ResetBoard(player);
        }
    }

    public void ResetBoard(Player player)
    {
        Board board = GetBoard(player);
        var activePiecesOnBoard = board.GetActivePiecesOnBoard();
        foreach (Piece piece in board.GetPiecesOnBoard())
        {
            if (!activePiecesOnBoard.Contains(piece))
            {
                activePiecesOnBoard.Add(piece);
            }
            piece.Reset();
            MovePieceToTile(player, piece, piece.GetInitialTile());
        }
    }

    public void MovePieceToTile(Player player, Piece piece, Tile nextTile)
    {
        Board board = GetBoard(player);
        board.MovePieceToTile(piece, nextTile);
        EventManager.Instance.Raise(new PieceMoveEvent{ piece = piece, tile = nextTile });
    }

    public void RemoveAllEnemies(Player player)
    {
        Board board = GetBoard(player);
        var enemyPieces = board.GetPiecesOnBoard().Where(piece => piece.IsEnemy()).ToArray();
        foreach (Piece enemy in enemyPieces)
        {
            RemovePieceFromBoard(player, enemy);
        }
    }

    public void RemoveAllPiecesFromBoard(Player player)
    {
        Board board = GetBoard(player);
        var pieces = board.GetPiecesOnBoard().ToArray();
        foreach (Piece piece in pieces)
        {
            RemovePieceFromBoard(player, piece);
        }
    }

    public void RemovePieceFromBoard(Player player, Piece piece)
    {
        Board board = GetBoard(player);
        board.RemovePieceFromBoard(piece);
        EventManager.Instance.Raise(new RemovePieceFromBoardEvent{ piece = piece });
    }

    public void AddPieceToBoard(Player player, Piece piece, int i, int j)
    {
        Board board = GetBoard(player);
        board.AddPieceToBoard(piece, i, j);
        EventManager.Instance.Raise(new AddPieceToBoardEvent { piece = piece, row = i, col = j });
    }
}
