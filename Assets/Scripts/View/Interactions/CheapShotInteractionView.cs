using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapShotInteractionView : InteractionView
{
    public Material GlowSword;
    private Material originalMat;

    TricksterView trickster;

    void Start()
    {
        var cheapShot = interaction as CheapShotSkill;
        trickster = cheapShot.caster.GetPieceView().GetComponentInChildren<TricksterView>() as TricksterView;
        originalMat = trickster.LSword.material;
        trickster.LSword.material = GlowSword;
        trickster.RSword.material = GlowSword;
    }

    public override void CleanUpInteraction()
    {
        // theres a similar bug to BarkSkin
        // see BarkSkinInteractionView
        trickster.LSword.material = originalMat;
        trickster.RSword.material = originalMat;
        Destroy(gameObject);
    }
}
