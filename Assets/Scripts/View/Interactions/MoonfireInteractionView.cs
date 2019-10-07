using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonfireInteractionView : InteractionView
{
    public int DelayUntilEndAnimation = 3;

    public override void CleanUpInteraction()
    {
        StartCoroutine(DelayThenPlayEnd());
    }

    IEnumerator DelayThenPlayEnd()
    {
        yield return new WaitForSeconds(DelayUntilEndAnimation);
        var animator = GetComponent<Animator>();
        animator.Play("Moon Fire End");
    }

    // Called by animator
    public void DestroyMoonfire()
    {
        Destroy(gameObject);
    }
}
