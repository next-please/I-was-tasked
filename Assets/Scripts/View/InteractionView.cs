using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionView : MonoBehaviour
{
    protected Interaction interaction;
    protected bool isCleaningUp = false;

    public void TrackInteraction(Interaction interaction)
    {
        this.interaction = interaction;
    }

    public virtual void CleanUpInteraction()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (interaction == null)
        {
            return;
        }

        if (!interaction.ProcessInteractionView())
        {
            if (!isCleaningUp)
            {
                isCleaningUp = true;
                CleanUpInteraction();
            }
        }
    }
}
