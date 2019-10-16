public class InfiniteState : State
{
    public override void OnStart(Piece piece, Board board)
    {
        ticksRemaining = 100;
    }

    public override void OnViewStart(PieceView pieceView)
    {
        pieceView.animator.Play("Idle", 0);
    }

    public override void OnTick(Piece piece, Board board) { }
}
