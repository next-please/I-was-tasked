using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickLogger : Tickable
{
    public Text text;
    public override void Tick(long tick)
    {
        text.text = tick.ToString();
    }
}
