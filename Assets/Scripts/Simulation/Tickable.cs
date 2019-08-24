using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tickable : MonoBehaviour
{
    void Start()
    {
        FixedClock.Instance.AddTickable(this);
    }

    public abstract void Tick(long tick);

    void OnDestroy()
    {
        FixedClock.Instance.RemoveTickable(this);
    }
}
