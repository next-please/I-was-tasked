using UnityEngine;

public class MoveAction : Action
{
    private Tile destination;

    public override bool ShouldTransitInto(Piece piece)
    {
        Piece target = piece.GetTarget();
        return (target != null && !target.IsDead());
    }

    public override void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 50 / piece.GetMovementSpeed();
        board.DeterminePieceLockedTile(piece);
        destination = piece.GetLockedTile();
    }

    public override void OnFinish(Piece piece, Board board)
    {
        board.MovePieceToTile(piece, destination);
    }

    // View Meta Info
    private float speedToTranslate = 0;
    public override void OnViewStart(PieceView pieceView)
    {
        // estimate how fast we need to move
        float timeToReachTile = ticksRemaining * FixedClock.Instance.deltaTime;
        float distanceToTile = 1;
        float speedToTranslate = distanceToTile / timeToReachTile;
    }

    public override void OnViewUpdate(PieceView pieceView)
    {
        // translate towards destination based on estimated speed
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        // just set position to the final destination (need to be consistent with the simulation)
    }
}

