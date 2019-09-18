using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction
{
    public abstract bool ProcessInteraction();
    public abstract void CleanUpInteraction();
}
