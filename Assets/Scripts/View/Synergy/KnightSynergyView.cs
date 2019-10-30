﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class KnightSynergyView : MonoBehaviour
{
    public Material ShieldEffectMaterial;
    public MeshRenderer ShieldRenderer;
    public VisualEffect vfx;

    private PieceView pieceView;
    private Material originalMaterial;

    void Start()
    {
        EventManager.Instance.AddListener<JobSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
        originalMaterial = ShieldRenderer.material;
        pieceView = GetComponentInParent<PieceView>();
        if (pieceView == null)
            Destroy(this);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<JobSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
    }

    void OnSynergyApplied(JobSynergyAppliedEvent e)
    {
        if (e.Job != Enums.Job.Knight)
            return;
        Piece piece = pieceView.piece;
        if (piece.IsOnBoard() && !piece.IsEnemy())
        {
            ShieldRenderer.material = ShieldEffectMaterial;
            vfx.SendEvent("Start");
        }
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Combat)
        {
            ShieldRenderer.material = originalMaterial;
            vfx.SendEvent("Stop");
        }
    }
}