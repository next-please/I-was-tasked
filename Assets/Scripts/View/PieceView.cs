using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PieceView : MonoBehaviour
{
    public Animator animator;
    public Piece piece = null; // piece that I'm trying to display
    public GameObject currentHPBar;
    private IViewAction prevViewAction;
    private int prevHP;

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
            style.fontSize = 24;
            Handles.Label(transform.position + Vector3.up * 0.5f, piece.GetName(), style);
            prevHP = piece.GetHitPoints();
        }
    }

    void Update()
    {
        if (piece == null)
        {
            return;
        }

        if (prevHP != piece.GetHitPoints())
        {
            UpdateCurrentHPBar();
        }

        if (piece.IsDead())
        {
            animator.Play("Death", 0);
            return;
        }

        IViewAction viewAction = piece.GetViewAction();
        if (prevViewAction != null)
        {
            prevViewAction.CallViewFinishIfNeeded(this);
        }
        viewAction.CallViewStartIfNeeded(this);
        viewAction.OnViewUpdate(this);
        prevViewAction = viewAction;
    }

    void OnPieceRemoved(RemovePieceFromBoardEvent e)
    {
        if (e.piece == piece)
            Destroy(gameObject);
    }

    public void UpdateCurrentHPBar()
    {
        float currentHPFraction = piece.GetHitPoints() / 100.0f;
        Vector3 temp = currentHPBar.transform.localScale;
        temp.x = currentHPFraction;
        currentHPBar.transform.localScale = temp;
        prevHP = piece.GetHitPoints();
    }
}
