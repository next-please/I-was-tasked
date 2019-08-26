using UnityEngine;

public class AttackAction : Action
{
    public override bool ShouldTransitInto(Piece piece)
    {
        Piece target = piece.GetTarget();
        return (target != null && !target.IsDead() && piece.CanAttackTarget());
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
