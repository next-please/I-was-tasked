using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class PriestSynergyView : MonoBehaviour
{
    public VisualEffect vfx;

    private PieceView pieceView;

    void Start()
    {
        EventManager.Instance.AddListener<JobSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
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
        if (e.Job != Enums.Job.Priest)
            return;
        Piece piece = pieceView.piece;
        if (piece.IsOnBoard() && !piece.IsEnemy())
        {
            vfx.SendEvent("Start");
        }
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Combat)
        {
            vfx.SendEvent("Stop");
        }
    }
}
