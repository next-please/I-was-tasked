using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    private int waitTime = 5;
    public WaitState(int time)
    {
        waitTime = time;
    }
    public override void OnStart(Piece piece, Board board) { ticksRemaining = waitTime; }
}
