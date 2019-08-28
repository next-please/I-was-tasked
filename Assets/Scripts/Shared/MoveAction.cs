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
    public override void OnViewStart(PieceView pieceView)
    {
        // Tile destination = pieceView.piece.GetLockedTile();
        // // estimate how fast we need to move

        // Vector3 tilePos = new Vector3(destination.GetRow(), 1, destination.GetCol());
        // Vector3 dir = (tilePos - pieceView.transform.position);
        // float distanceTotile = dir.magnitude;
        // float timeToReachTile = ticksRemaining * FixedClock.Instance.deltaTime;
        // float distanceToTile = 1;
        // float speedToTranslate = distanceToTile / timeToReachTile;
        // dir.Normalize();
    }

    Tile destination = null;

    public override void OnViewUpdate(PieceView pieceView)
    {
        destination = pieceView.piece.GetLockedTile();
        Vector3 tilePos = new Vector3(destination.GetRow(), 1, destination.GetCol());
        float distanceTotile = (tilePos - pieceView.transform.position).magnitude;
        float timeToReachTile = ticksRemaining * FixedClock.Instance.deltaTime;
        float speedToTranslate = distanceTotile / timeToReachTile;
        // translate towards destination based on estimated speed
        pieceView.transform.LookAt(tilePos);
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
