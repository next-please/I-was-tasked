using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePiece : Piece
{
    // Placeholder Constructor; actual Melee Piece would be more complex in attributes.
    public MeleePiece(string name, int hitPoints, int attackDamage, bool isEnemy)
    {
        SetName(name);
        SetHitPoints(hitPoints);
        SetAttackDamage(attackDamage);
        SetAttackRange(1);
        SetIsEnemy(isEnemy);
    }

    public override void AttackTarget()
    {
        Piece target = GetTarget();
        target.SetHitPoints(target.GetHitPoints() - GetAttackDamage());
    }

    void FindNewTarget(Board board)
    {
        Piece currentTarget = GetTarget();
        if (currentTarget != null && !currentTarget.IsDead())
        {
            SetTarget(currentTarget);
            return;
        }

        List<Piece> activePiecesOnBoard = board.GetActivePiecesOnBoard();
        Piece nearestTarget = FindNearestTarget(activePiecesOnBoard); // Find a new Target (if any).
        if (nearestTarget == null) // There are no more enemies; game is Resolved.
        {
            Debug.Log("There are no more enemies for " + GetName() + " to target. Game is Resolved.");
        }
        else
        {
            Debug.Log(GetName() + " now has a Target of " + nearestTarget.GetName() + " and is going to move closer to it.");
        }
        SetTarget(nearestTarget);
    }

    public override Action DecideNextAction(Board board)
    {
        FindNewTarget(board);
        if (GetTarget() == null || GetTarget().IsDead())
        {
            return new InfiniteAction();
        }
        if (CanAttackTarget())
        {
            return new AttackAction(10);
        }
        else
        {
            return new MoveAction(10, this, board);
        }
    }
}
