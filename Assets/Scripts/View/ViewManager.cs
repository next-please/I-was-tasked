﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Nextplease.IWT;

public class ViewManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CharacterPrefabLoader characterPrefabLoader;
    public InteractionPrefabLoader interactionPrefabLoader;
    public GameObject FriendlyPieceViewPrefab;
    public GameObject EnemyPieceViewPrefab;
    public GameObject BoardPlayerOne;
    public GameObject BoardPlayerTwo;
    public GameObject BoardPlayerThree;
    public Transform[] TileOriginTransforms;
    public GameObject HighlightPrefab;

    private List<GameObject> highlightPool = new List<GameObject>();

    static float TileSize = 1;
    static float boardOffset = 20;
    struct BoardDimension
    {
        public int rows;
        public int cols;
        public Vector3 origin;
    }

    private static BoardDimension[] boardDimensions = new BoardDimension[3];

    void OnEnable()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
        EventManager.Instance.AddListener<AddInteractionToProcessEvent>(OnInteractionAdded);
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnAddPiece);
        EventManager.Instance.AddListener<RemovePieceFromBoardEvent>(OnRemovePiece);
        EventManager.Instance.AddListener<PieceMoveEvent>(OnPieceMove);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
        EventManager.Instance.RemoveListener<AddInteractionToProcessEvent>(OnInteractionAdded);
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnAddPiece);
        EventManager.Instance.RemoveListener<RemovePieceFromBoardEvent>(OnRemovePiece);
        EventManager.Instance.RemoveListener<PieceMoveEvent>(OnPieceMove);
    }

    public void OnBoardCreated(Board gameBoard, Player player)
    {
        int rows = gameBoard.GetNumRows();
        int cols = gameBoard.GetNumCols();
        bool toggle = false;
        Transform startTransform = TileOriginTransforms[(int)player];
        boardDimensions[(int)player] = new BoardDimension { rows = rows, cols = cols, origin = startTransform.position };

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                GameObject tile = GetPlayerGameBoard(player).transform.GetChild(i * rows + j).gameObject;
                TileView tileView = tile.GetComponent<TileView>();
                tileView.TrackTile(gameBoard.GetTile(j, i));
                toggle = !toggle;
            }
            toggle = !toggle;
        }
    }

    public void OnPieceAdded(AddPieceToBoardEvent e)
    {
        Piece piece = e.piece;
        Player boardOwner = e.player;
        GameObject pieceViewPrefab = piece.IsEnemy() ? EnemyPieceViewPrefab : FriendlyPieceViewPrefab;

        Vector3 tileWorldPos = CalculateTileWorldPosition(e.tile);
        tileWorldPos.y = 0.5f;
        if (piece.size == Enums.Size.Small)
        {
            tileWorldPos.y = -0.4f;
        }
        if (piece.size == Enums.Size.Big)
        {
            tileWorldPos.y = 1.3f;
        }
        GameObject pieceObj = Instantiate(pieceViewPrefab, tileWorldPos, Quaternion.identity);
        PieceView pieceView = pieceObj.GetComponent<PieceView>();
        pieceView.TrackPiece(piece);
        pieceView.InstantiateModelPrefab(characterPrefabLoader.GetPrefab(piece), boardOwner);
        pieceObj.transform.parent = transform;
    }

    public void OnInteractionAdded(AddInteractionToProcessEvent e)
    {
        Interaction interaction = e.interaction;
        if (interaction.interactionPrefab == Enums.InteractionPrefab.None)
            return;
        GameObject interactionViewPrefab = interactionPrefabLoader.GetPrefab(interaction.interactionPrefab);
        GameObject interactionObj = Instantiate(interactionViewPrefab, Vector3.zero, Quaternion.identity);
        InteractionView interactionView = interactionObj.GetComponent<InteractionView>();
        interactionView.TrackInteraction(interaction);
        interaction.TrackInteractionView(interactionView);
        interactionObj.transform.parent = transform;
    }

    public static Vector3 CalculateTileWorldPosition(Tile tile)
    {
        Player boardOwner = tile.GetBoard().GetOwner();
        BoardDimension boardDimension = boardDimensions[(int)boardOwner];
        int i = tile.GetRow();
        int j = tile.GetCol();
        float rotation = (int)boardOwner * 45;
        Vector3 up = Quaternion.Euler(0, rotation, 0) * new Vector3(0, 0, 1);
        Vector3 right = Quaternion.Euler(0, rotation, 0) * new Vector3(1, 0, 0);
        return boardDimension.origin + (right * i + up * j) * TileSize;
    }

    private GameObject GetPlayerGameBoard(Player player)
    {
        switch (player)
        {
            case Player.Zero:
                return BoardPlayerOne;
            case Player.One:
                return BoardPlayerTwo;
            case Player.Two:
                return BoardPlayerThree;
            default:
                return BoardPlayerOne;
        }
    }

    private void OnAddPiece(AddPieceToBoardEvent e)
    {
        UpdateHighlights(e.player);
    }

    private void OnRemovePiece(RemovePieceFromBoardEvent e)
    {
        UpdateHighlights(e.player);
    }

    private void OnPieceMove(PieceMoveEvent e)
    {
        UpdateHighlights(e.tile.GetBoard().GetOwner());
    }

    void UpdateHighlights(Player player)
    {
        if (player == RoomManager.GetLocalPlayer())
        {
            foreach (GameObject highlight in highlightPool)
            {
                highlight.SetActive(false);
            }
            List<Piece> pieces = inventoryManager.GetExcessPieces(player);
            if (highlightPool.Count < pieces.Count)
            {
                GameObject highlight = GameObject.Instantiate(HighlightPrefab);
                highlight.SetActive(false);
                highlightPool.Add(highlight);
                highlight.transform.Rotate(Vector3.up, 45 * (int)player, Space.World);
            }

            for (int i = 0; i < pieces.Count; ++i)
            {
                Piece piece = pieces[i];
                Tile tile = piece.GetCurrentTile();
                if (tile == null)
                    continue;
                Vector3 position = CalculateTileWorldPosition(tile);
                position.y = 0.75f;
                GameObject highlight = highlightPool[i];
                highlight.SetActive(true);
                highlight.transform.position = position;
            }
        }
    }
}
