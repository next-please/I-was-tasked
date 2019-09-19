using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasFullMP : Predicate
{
    public bool IsTrue(Piece piece, Board board)
    {
        return (piece.GetCurrentManaPoints() == piece.GetMaximumManaPoints());
    }
}
