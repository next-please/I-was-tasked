using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public GameObject FriendlyPieceViewPrefab;
    public GameObject EnemyPieceViewPrefab;
    public GameObject TileViewPrefab;
    public Material White;
    public Material Black;
    public float TileSize = 1;

    void OnEnable()
    {
        EventManager.Instance.AddListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<AddPieceToBoardEvent>(OnPieceAdded);
    }

    public void OnBoardCreated(Board gameBoard)
    {
        int rows = gameBoard.GetNumRows();
        int cols = gameBoard.GetNumCols();
        bool toggle = false;
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                GameObject tile = Instantiate(TileViewPrefab, new Vector3(i, 0, j) * TileSize, Quaternion.identity);
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

        GameObject pieceViewPrefab = piece.IsEnemy() ? EnemyPieceViewPrefab : FriendlyPieceViewPrefab;
        GameObject pieceObj = Instantiate(pieceViewPrefab, new Vector3(i, 1, j) * TileSize, Quaternion.identity);
        PieceView pieceView = pieceObj.GetComponent<PieceView>();
        pieceView.TrackPiece(piece);
        pieceObj.transform.parent = transform;
    }
}
