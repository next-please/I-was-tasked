using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class OrcSynergyView : MonoBehaviour
{
    public VisualEffect vfx;
    private PieceView pieceView;

    void Start()
    {
        EventManager.Instance.AddListener<RaceSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
        pieceView = GetComponentInParent<PieceView>();
        if (pieceView == null)
            Destroy(this);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<RaceSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
    }

    void OnSynergyApplied(RaceSynergyAppliedEvent e)
    {
         if (e.Race != Enums.Race.Orc)
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
