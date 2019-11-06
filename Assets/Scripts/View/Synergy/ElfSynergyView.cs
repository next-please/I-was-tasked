using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class ElfSynergyView : MonoBehaviour
{
    public VisualEffect vfx;
    private PieceView pieceView;

    void Start()
    {
        EventManager.Instance.AddListener<RaceSynergyAppliedEvent>(OnSynergyApplied);
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

        bool hasSynergy = SynergyManager.GetInstance().HasSynergy(Enums.Race.Elf);
        ToggleVfx(hasSynergy);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<RaceSynergyAppliedEvent>(OnSynergyApplied);
    }

    void OnSynergyApplied(RaceSynergyAppliedEvent e)
    {
         if (e.Race != Enums.Race.Elf)
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
