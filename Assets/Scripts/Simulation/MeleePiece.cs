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
        SetMovementSpeed(1);
        this.action = CreateAndConnectActions();
    }

    public override void AttackTarget()
    {
        Piece target = GetTarget();
        target.SetHitPoints(target.GetHitPoints() - GetAttackDamage());
    }

    Action CreateAndConnectActions()
    {
        FindNewTargetAction findTarget = new FindNewTargetAction();
        MoveAction move = new MoveAction();
        AttackAction attack = new AttackAction();
        InfiniteAction inf = new InfiniteAction();

        findTarget.AddNextAction(attack); // after finding, we try to attack
        findTarget.AddNextAction(move); // if we cant attack, we try to move towards target
        findTarget.AddNextAction(inf); // we cant find anything

        attack.AddNextAction(attack); // attack same target
        // attack.AddNextAction(move); // uncomment to chase
        attack.AddNextAction(findTarget); // find new target

        // uncomment the top 2 for chasing behaviour
        // move.AddNextAction(attack); // attack same target
        // move.AddNextAction(move); // we may have to chase
        move.AddNextAction(findTarget); // find new target

        return findTarget; // our initial action is find
    }
}
