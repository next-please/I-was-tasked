using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tickable : MonoBehaviour
{

    // please call this if you redefine Start
    // base.Start()
    protected void Start()
    {
        FixedClock.Instance.AddTickable(this);
    }

    public abstract void Tick(long tick);

    // please call this if you redefine OnDestroy
    // base.OnDestroy()
    protected void OnDestroy()
    {
        FixedClock.Instance.RemoveTickable(this);
    }
}
