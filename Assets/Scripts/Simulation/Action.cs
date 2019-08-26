using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    protected int ticksRemaining;

    public Action(int ticksRemaining)
    {
        this.ticksRemaining = ticksRemaining;
    }

    public virtual void OnTick(Piece piece, Board board)
    {
        ticksRemaining--;
    }

    public virtual void OnFinish(Piece piece, Board board)
    {
    }

    public bool hasFinished()
    {
        return ticksRemaining <= 0;
    }
}

public class InfiniteAction : Action
{
    public InfiniteAction() : base(1) { }
    public override void OnTick(Piece piece, Board board) { }
}

public class AttackAction : Action
{
    public AttackAction(int ticksRemaining)
        : base(ticksRemaining)
    {
    }

    public override void OnTick(Piece piece, Board board)
    {
        base.OnTick(piece, board);
        if (piece.GetTarget().IsDead())
        {
            ticksRemaining = 0;
        }
    }

    public override void OnFinish(Piece piece, Board board)
    {
        Piece target = piece.GetTarget();
        if (!target.IsDead())
        {
            piece.AttackTarget();
            Debug.Log(piece.GetName() + " has attacked " + target.GetName() + " for " + piece.GetAttackDamage() + " DMG, whose HP has dropped to " + target.GetHitPoints() + " HP."); ;
            if (target.IsDead())
            {
                board.DeactivatePieceOnBoard(target);
                Debug.Log(target.GetName() + " has died and " + piece.GetName() + " is no longer attacking it.");

            }
        }
    }
}

public class MoveAction : Action
{
    public MoveAction(int ticksRemaining, Piece piece, Board board)
        : base(ticksRemaining)
    {
        board.DeterminePieceLockedTile(piece);
    }

    public override void OnFinish(Piece piece, Board board)
    {
        board.MovePieceToTile(piece, piece.GetLockedTile());
    }
}

