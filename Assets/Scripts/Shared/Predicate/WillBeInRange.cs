using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillBeInRange : Predicate
{
    public bool IsTrue(Piece piece)
    {
        Tile targetTile = piece.GetTarget().GetLockedTile();
        if (targetTile == null)
            return false;
        return (piece.GetCurrentTile().DistanceToTile(targetTile) <= piece.GetAttackRange());
    }
}
