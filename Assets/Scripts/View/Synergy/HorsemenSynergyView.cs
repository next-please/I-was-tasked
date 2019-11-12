using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class HorsemenSynergyView : MonoBehaviour
{
    public VisualEffect vfxWar;
    public VisualEffect vfxLife;
    public VisualEffect vfxEssence;
    public VisualEffect vfxUnity;
    private VisualEffect[] vfxArray;
    private PieceView pieceView;

    void Start()
    {
        pieceView = GetComponentInParent<PieceView>();
        vfxArray = new VisualEffect[] { vfxWar, vfxLife, vfxEssence, vfxUnity };
        if (pieceView == null)
        {
            Destroy(this);
            return;
        }

        Piece piece = pieceView.piece;
        if (piece.GetTitle().Equals("Horseman"))
        {
            int index;
            switch (piece.GetName())
            {
                case "Horseman of War":
                    index = 0;
                    break;
                case "Horseman of Life":
                    index = 1;
                    break;
                case "Horseman of Essence":
                    index = 2;
                    break;
                case "Horseman of Unity3D":
                    index = 3;
                    break;
                default:
                    index = 0;
                    break;
            }
            ToggleVfx(index);
        }
    }

    void ToggleVfx(int index)
    {
        string message = "Start";
        vfxArray[index].SendEvent(message);
    }
}
