﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PieceView : MonoBehaviour
{
    public Animator animator;
    public Piece piece = null; // piece that I'm trying to display
    public GameObject currentHPBar;
    public GameObject currentMPBar;
    private IViewState prevViewAction;
    private int prevHP;
    private int prevMP;

    private void Start()
    {
        SetCurrentMPBar(0.0f);
    }
    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
    }

    void OnEnable()
    {
        EventManager.Instance.AddListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
        EventManager.Instance.AddListener<PieceMoveEvent>(OnPieceMove);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<RemovePieceFromBoardEvent>(OnPieceRemoved);
        EventManager.Instance.RemoveListener<PieceMoveEvent>(OnPieceMove);
    }

    void OnDrawGizmos()
    {
        if (piece != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 24;
            // Handles.Label(transform.position + Vector3.up * 0.5f, piece.GetViewState().ToString(), style);
        }
    }

    void Update()
    {
        if (piece == null)
        {
            return;
        }

        if (prevHP != piece.GetCurrentHitPoints())
        {
            UpdateCurrentHPBar();
        }

        if (prevMP != piece.GetCurrentManaPoints())
        {
            UpdateCurrentMPBar();
        }

        if (piece.IsDead())
        {
            animator.Play("Death", 0);
            return;
        }

        IViewState viewAction = piece.GetViewState();
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

    void OnPieceMove(PieceMoveEvent e)
    {
        if (e.piece == piece)
        {
            int i = e.tile.GetRow();
            int j = e.tile.GetCol();
            transform.position = new Vector3(i, 1, j);
            transform.rotation = Quaternion.identity;
        }
    }

    public void SetCurrentHPBar(float hp)
    {
        float currentHPFraction = hp / piece.GetMaximumHitPoints();
        Vector3 temp = currentHPBar.transform.localScale;
        temp.x = currentHPFraction;
        currentHPBar.transform.localScale = temp;
        prevHP = piece.GetCurrentHitPoints();
    }
    public void SetCurrentMPBar(float mp)
    {
        float currentMPFraction = mp / piece.GetMaximumManaPoints();
        Vector3 temp = currentMPBar.transform.localScale;
        temp.x = currentMPFraction;
        currentMPBar.transform.localScale = temp;
        prevMP = piece.GetCurrentManaPoints();
    }

    public void UpdateCurrentHPBar()
    {
        SetCurrentHPBar(piece.GetCurrentHitPoints());
    }

    public void UpdateCurrentMPBar()
    {
        SetCurrentMPBar(piece.GetCurrentManaPoints());
    }
}
