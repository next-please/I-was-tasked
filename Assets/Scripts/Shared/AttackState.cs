using UnityEngine;

public class AttackState : State
{
    public override void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 50; // 1.0 second to attack

        Piece target = piece.GetTarget();
        if (!target.IsDead())
        {
            target.SetHitPoints(target.GetHitPoints() - piece.GetAttackDamage());
            Debug.Log(piece.GetName() + " has attacked " + target.GetName() + " for " + piece.GetAttackDamage() + " DMG, whose HP has dropped to " + target.GetHitPoints() + " HP.");
        }
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
        pieceView.transform.LookAt(new Vector3(targetTile.GetRow(), 1, targetTile.GetCol()));
        pieceView.animator.Play("Attack", 0);
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }
}
