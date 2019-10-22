using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingOfNatureInteractionView : InteractionView
{
    public override void CleanUpInteraction()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
