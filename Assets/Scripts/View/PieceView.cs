using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PieceView : MonoBehaviour
{
    public Animator animator;
    public Piece piece = null; // piece that I'm trying to display
    private IViewAction prevViewAction;
    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
    }

    void OnDrawGizmos()
    {
        if (piece != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 16;
            Handles.Label(transform.position + Vector3.up * 0.5f, piece.GetName(), style);
        }
    }

    void Update()
    {
        if (piece == null)
        {
            return;
        }

        if (piece.IsDead())
        {
            animator.Play("Death", 0);
            return;
        }

        IViewAction viewAction = piece.GetViewAction();
        if (prevViewAction != null)
            prevViewAction.CallViewFinishIfNeeded(this);
        viewAction.CallViewStartIfNeeded(this);
        viewAction.OnViewUpdate(this);
        prevViewAction = viewAction;
    }

    void OnPieceRemoved(RemovePieceFromBoardEvent e)
    {
        if (e.piece == piece)
            Destroy(gameObject);
    }
}
