using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSynergyView : MonoBehaviour
{
    public Material ShieldEffectMaterial;
    public MeshRenderer ShieldRenderer;

    private PieceView pieceView;
    private Material originalMaterial;

    void Start()
    {
        EventManager.Instance.AddListener<KnightSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.AddListener<ExitPhaseEvent>(OnExitPhase);
        originalMaterial = ShieldRenderer.material;
        pieceView = GetComponentInParent<PieceView>();
        if (pieceView == null)
            Destroy(this);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<KnightSynergyAppliedEvent>(OnSynergyApplied);
        EventManager.Instance.RemoveListener<ExitPhaseEvent>(OnExitPhase);
    }

    void OnSynergyApplied(KnightSynergyAppliedEvent e)
    {
        Piece piece = pieceView.piece;
        if (piece.IsOnBoard() && !piece.IsEnemy())
            ShieldRenderer.material = ShieldEffectMaterial;
    }

    void OnExitPhase(ExitPhaseEvent e)
    {
        if (e.phase == Phase.Combat)
        {
            ShieldRenderer.material = originalMaterial;
        }
    }
}
