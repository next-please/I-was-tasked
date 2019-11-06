﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class DruidSynergyView : MonoBehaviour
{
    public VisualEffect vfx;

    private PieceView pieceView;

    void Start()
    {
        EventManager.Instance.AddListener<JobSynergyAppliedEvent>(OnSynergyApplied);
        pieceView = GetComponentInParent<PieceView>();

        if (pieceView == null)
        {
            Destroy(this);
            return;
        }

        Piece piece = pieceView.piece;
        if (piece.IsEnemy())
        {
            Destroy(this);
            return;
        }

        bool hasSynergy = SynergyManager.GetInstance().HasSynergy(Enums.Job.Druid);
        ToggleVfx(hasSynergy);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<JobSynergyAppliedEvent>(OnSynergyApplied);
    }

    void OnSynergyApplied(JobSynergyAppliedEvent e)
    {
        if (e.Job != Enums.Job.Druid)
            return;
        Piece piece = pieceView.piece;
        if (piece.IsOnBoard() && !piece.IsEnemy())
        {
            ToggleVfx(e.Applied);
        }
    }

    void ToggleVfx(bool apply)
    {
        string message = apply ? "Start" : "Stop";
        vfx.SendEvent(message);
    }
}
