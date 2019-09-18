using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction
{
    public Enums.InteractionPrefab interactionPrefab;
    public InteractionView interactionView;

    public abstract bool ProcessInteraction();
    public abstract void CleanUpInteraction();
    public abstract bool ProcessInteractionView();

    public void TrackInteractionView(InteractionView interactionView)
    {
        this.interactionView = interactionView;
    }
}
