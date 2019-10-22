using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeShiftInteractionView : InteractionView
{
    // Start is called before the first frame update
    void Start()
    {
        ShapeshiftLingeringEffect effect = interaction as ShapeshiftLingeringEffect;
        PieceView pieceView = effect.caster.GetPieceView();
        DruidOfWildView dofV = pieceView.GetComponentInChildren<DruidOfWildView>();

        if (dofV == null) // currently enemies are using this...
            return;

        transform.parent = dofV.Head;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
