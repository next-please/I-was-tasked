using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class PieceView : MonoBehaviour
{
    public Animator animator;
    public Piece piece = null; // piece that I'm trying to display
    public GameObject currentHPBar;
    public TextMeshPro jobTextMesh; // todo: placeholder, remove later
    private IViewState prevViewAction;
    private int prevHP;

    public void TrackPiece(Piece piece)
    {
        this.piece = piece;
    }

    // todo: placeholder, remove later
    public void SetJobText(string job, string race)
    {
        if (jobTextMesh == null)
        {
            return;
        }
        jobTextMesh.text = job + " " + race;
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
            Handles.Label(transform.position + Vector3.up * 0.5f, piece.GetName(), style);
            prevHP = piece.GetHitPoints();
        }
    }

    // todo: move to utility
    void LateUpdate()
    {
        if (jobTextMesh != null)
        {
            //     jobTextMesh.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            //    Camera.main.transform.rotation * Vector3.up);

            //     currentHPBar.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            //     Camera.main.transform.rotation * Vector3.up);

            jobTextMesh.transform.rotation = Camera.main.transform.rotation;
            currentHPBar.transform.rotation = Camera.main.transform.rotation;
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

    public void UpdateCurrentHPBar()
    {
        float currentHPFraction = piece.GetHitPoints() / 100.0f;
        Vector3 temp = currentHPBar.transform.localScale;
        temp.x = currentHPFraction;
        currentHPBar.transform.localScale = temp;
        prevHP = piece.GetHitPoints();
    }
}
