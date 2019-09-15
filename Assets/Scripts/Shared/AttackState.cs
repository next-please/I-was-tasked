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
            target.SetCurrentHitPoints(target.GetCurrentHitPoints() - piece.GetAttackDamage());
            if (piece.GetLifestealPercentage() > 0) // Undead synergy
            {
                piece.SetCurrentHitPoints(Math.Min(piece.GetMaximumHitPoints(),
                    (int) Math.Floor((piece.GetCurrentHitPoints() + piece.GetAttackDamage()*piece.GetLifestealPercentage()))));
            }
            if (target.GetRecoilPercentage() > 0) // Knight synergy
            {
                piece.SetCurrentHitPoints((int) Math.Ceiling(piece.GetCurrentHitPoints() - piece.GetAttackDamage() * target.GetRecoilPercentage()));
            }
            target.SetCurrentManaPoints(target.GetCurrentManaPoints() + target.GetManaPointsGainedOnDamaged());
            Debug.Log(piece.GetName() + " has attacked " + target.GetName() + " for " + piece.GetAttackDamage() + " DMG, whose HP has dropped to " + target.GetCurrentHitPoints() + " HP.");
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
            target.GetLockedTile();
        }
        if (targetTile == null)
        {
            Debug.Log("No target to look at, See AttackState.cs");
            return;
        }
        pieceView.transform.LookAt(new Vector3(targetTile.GetRow(), 0.5f, targetTile.GetCol()));
        pieceView.animator.Play("Attack", 0);
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }
}
