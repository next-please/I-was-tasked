using UnityEngine;
using System;

public class AttackState : State
{
    public override void OnStart(Piece piece, Board board)
    {
        Piece target = piece.GetTarget();
        piece.SetCurrentManaPoints(piece.GetCurrentManaPoints() + piece.GetManaPointsGainedOnAttack());

        // Where 50 Ticks => 1.0 Attacks per Second (APS); attack speed of pieces currently
        // ranges from [1, 10], so an attack speed of 5 will give the Piece 1.0 APS.
        // Also, this does not affect the animation speed.
        ticksRemaining = 250 / piece.GetAttackSpeed();

        if (!target.IsDead())
        {
            Interaction attack;
            if (piece.GetAttackRange() > 1 && piece.GetClass() != Enums.Job.Spearman)
            {
                // Projectiles take 10 ticks to move 1 Tile. This should change later
                // when we know how fast each projectile is supposed to travel.
                int ticksToHit = 10 * piece.GetCurrentTile().DistanceToTile(target.GetCurrentTile());
                attack = new RangedAttack(piece, target, piece.GetAttackDamage(), ticksToHit);
            }
            else
            {
                attack = new MeleeAttack(piece, target, piece.GetAttackDamage());
            }
            board.AddInteractionToProcess(attack);
        }
        else
        {
            ticksRemaining = 0;
        }
    }

    public override void OnTick(Piece piece, Board board)
    {
        base.OnTick(piece, board);
    }

    public override void OnFinish(Piece piece, Board board)
    {
        Piece target = piece.GetTarget();
        if (target.IsDead())
        {
            board.DeactivatePieceOnBoard(target);
            Debug.Log(target.GetName() + " has died and " + piece.GetName() + " is no longer attacking it.");
        }
    }

    public override void OnViewStart(PieceView pieceView)
    {
        Piece target = pieceView.piece.GetTarget();
        Tile targetTile = target.GetCurrentTile();
        if (targetTile == null)
        {
            targetTile = target.GetLockedTile();
        }

        if (targetTile == null)
        {
            Debug.Log("No target to look at, See AttackState.cs");
            return;
        }
        pieceView.LookAtTile(targetTile);
        pieceView.animator.Play("Attack", 0);
        pieceView.pieceSounds.PlayAttackSound();
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }
}
