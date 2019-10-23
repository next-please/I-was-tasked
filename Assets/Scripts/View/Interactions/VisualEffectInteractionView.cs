using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class VisualEffectInteractionView : InteractionView
{
    public VisualEffect effect;
    public float SecondsTillDespawn = 3;
    public override void CleanUpInteraction()
    {
        effect.SendEvent("OnEnd");
        StartCoroutine(DestroyInSeconds(SecondsTillDespawn));
    }

    IEnumerator DestroyInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
