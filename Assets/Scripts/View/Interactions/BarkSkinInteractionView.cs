using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkSkinInteractionView : InteractionView
{
    public Material barkMaterial;

    private Material originalMat;
    private ProtectorOfEarthView poeV;

    void Start()
    {
        BarkskinLingeringEffect effect = interaction as BarkskinLingeringEffect;
        PieceView pieceView = effect.caster.GetPieceView();
        poeV = pieceView.GetComponentInChildren<ProtectorOfEarthView>();
        originalMat = poeV.meshRenderer.material;
        poeV.meshRenderer.material = barkMaterial;
    }

    public override void CleanUpInteraction()
    {
        // there's a bug when there is more than 1 bark skin applied
        // this will remove the bark skin effect that was triggered later
        // this shouldn't be a problem as we should only trigger 1 bark skin
        // should tweak the mana
        poeV.meshRenderer.material = originalMat;
    }
}
