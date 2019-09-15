﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public CharacterPrefabLoader characterPrefabLoader;
    public GameObject FriendlyPieceViewPrefab;
    public GameObject EnemyPieceViewPrefab;
    public GameObject TileViewPrefab;
    public Material White;
    public Material Black;

    static float TileSize = 1;
    struct BoardDimension
    {
        public int rows;
        public int cols;
        public Vector3 startPos;
    }

    private static BoardDimension[] boardDimensions = new BoardDimension[3];

    void OnEnable()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    public void OnBoardCreated(Board gameBoard, Player player)
    {
        int rows = gameBoard.GetNumRows();
        int cols = gameBoard.GetNumCols();
        bool toggle = false;
        Vector3 startPos = (Vector3.right * rows * TileSize + Vector3.right * 2) * (int)player; // board + offset
        boardDimensions[(int)player] = new BoardDimension{ rows = rows, cols = cols, startPos = startPos };
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                GameObject tile = Instantiate(TileViewPrefab, startPos + new Vector3(i, 0, j) * TileSize, Quaternion.identity);
                TileView tileView = tile.GetComponent<TileView>();
                tileView.TrackTile(gameBoard.GetTile(i, j));

                Renderer rend = tile.GetComponent<Renderer>();
                rend.material = toggle ? White : Black;
                toggle = !toggle;
            }
            toggle = !toggle;
        }
    }

    public void OnPieceAdded(AddPieceToBoardEvent e)
    {
        Piece piece = e.piece;
        int i = e.row;
        int j = e.col;
        Player player = e.player;

        Vector3 startPos = (Vector3.right * e.board.GetNumRows() * TileSize + Vector3.right * 2) * (int)player; // board + offset
        GameObject pieceViewPrefab = piece.IsEnemy() ? EnemyPieceViewPrefab : FriendlyPieceViewPrefab;
        GameObject pieceObj = Instantiate(pieceViewPrefab, startPos + new Vector3(i, 0.5f, j) * TileSize, Quaternion.identity);
        PieceView pieceView = pieceObj.GetComponent<PieceView>();
        pieceView.TrackPiece(piece);
        pieceView.InstantiateModelPrefab(characterPrefabLoader.GetPrefab(piece));
        pieceObj.transform.parent = transform;
    }

    public static Vector3 CalculateTileWorldPosition(Tile tile)
    {
        Player boardOwner = tile.GetBoard().GetOwner();
        BoardDimension boardDimension = boardDimensions[(int) boardOwner];
        int i = tile.GetRow();
        int j = tile.GetCol();
        return boardDimension.startPos + new Vector3(i, 0, j) * TileSize;
    }
}
