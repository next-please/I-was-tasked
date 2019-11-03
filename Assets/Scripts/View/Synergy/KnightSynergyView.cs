using System.Collections;
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
        originalMaterial = ShieldRenderer.material;

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

        bool hasSynergy = SynergyManager.GetInstance().HasSynergy(Enums.Job.Knight);
        ToggleVfx(hasSynergy);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<JobSynergyAppliedEvent>(OnSynergyApplied);
    }

    void OnSynergyApplied(JobSynergyAppliedEvent e)
    {
        if (e.Job != Enums.Job.Knight)
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
        ShieldRenderer.material = apply ? ShieldEffectMaterial : originalMaterial;
    }
}
