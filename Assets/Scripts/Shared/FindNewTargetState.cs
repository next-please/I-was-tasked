using System.Collections.Generic;
using UnityEngine;

public class FindNewTargetState : State
{
    // Hack, use OnTick for the case when this is the first action to be called, to be improved
    // this allows this action to be run once and also as the first action
    public override void OnTick(Piece piece, Board board)
    {
        ticksRemaining = 0; // code will run once and then finish, see Piece#Process
        Piece nearestTarget = board.FindNearestTarget(piece); // Find a new Target (if any).
        if (nearestTarget == null) // There are no more enemies; game is Resolved.
        {
            Debug.Log("There are no more enemies for " + piece.GetName() + " to target. Game is Resolved.");
        }
        else
        {
            Debug.Log(piece.GetName() + " now has a Target of " + nearestTarget.GetName() + " and is going to move closer to it.");
        }
        piece.SetTarget(nearestTarget);
    }
}
