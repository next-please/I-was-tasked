using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFindNextTile : Predicate
{
    public bool IsTrue(Piece piece, Board board)
    {
        return board.CanDeterminePieceLockedTile(piece);
    }
}
