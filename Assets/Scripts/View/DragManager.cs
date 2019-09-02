using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public BoardManager boardManager;
    public Player player;
    private Piece draggedPiece;
    void OnEnable()
    {
        EventManager.Instance.AddListener<PieceDragEvent>(OnPieceDrag);
        EventManager.Instance.AddListener<PieceDropOnBoardEvent>(OnPieceDrop);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<PieceDragEvent>(OnPieceDrag);
        EventManager.Instance.RemoveListener<PieceDropOnBoardEvent>(OnPieceDrop);
    }

    public void OnPieceDrag(PieceDragEvent e)
    {
        draggedPiece = e.piece;
    }

    public void OnPieceDrop(PieceDropOnBoardEvent e)
    {
        if (e.tile == null || e.tile.IsOccupied())
        {
            return;
        }
        boardManager.AddPieceToBoard(player, draggedPiece, e.tile.GetRow(), e.tile.GetCol());
    }
}
