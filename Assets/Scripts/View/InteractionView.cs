using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionView : MonoBehaviour
{
    private Interaction interaction;

    public void TrackInteraction(Interaction interaction)
    {
        this.interaction = interaction;
    }

    public void CleanUpInteraction()
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
            CleanUpInteraction();
        }
    }
}
