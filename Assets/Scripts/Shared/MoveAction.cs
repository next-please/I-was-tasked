using UnityEngine;

public class MoveAction : Action
{

    public override bool ShouldTransitInto(Piece piece)
    {
        Piece target = piece.GetTarget();
        return (target != null && !target.IsDead());
    }

    public override void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 50 / piece.GetMovementSpeed();
        board.DeterminePieceLockedTile(piece);
    }

    public override void OnFinish(Piece piece, Board board)
    {
        board.MovePieceToTile(piece, piece.GetLockedTile());
    }

    // View Meta Info
    Tile destination = null;
    float speedToTranslate = 0;
    public override void OnViewStart(PieceView pieceView)
    {
        destination = pieceView.piece.GetLockedTile();
        Vector3 tilePos = new Vector3(destination.GetRow(), 1, destination.GetCol());
        float distanceTotile = (tilePos - pieceView.transform.position).magnitude;
        float timeToReachTile = ticksRemaining * FixedClock.Instance.deltaTime;
        speedToTranslate = distanceTotile / timeToReachTile;
        pieceView.transform.LookAt(tilePos);
    }

    public override void OnViewUpdate(PieceView pieceView)
    {
        // translate towards destination based on estimated speed
        pieceView.transform.Translate(Vector3.forward * speedToTranslate * Time.deltaTime);
    }

    public override void OnViewFinish(PieceView pieceView)
    {
        // just set position to the final destination (need to be consistent with the simulation)
        // estimate how fast we need to move
        Vector3 tilePos = new Vector3(destination.GetRow(), 1, destination.GetCol());
        pieceView.transform.position = tilePos;
    }
}
