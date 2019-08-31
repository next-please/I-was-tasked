public class InfiniteState : State
{
    public override void OnStart(Piece piece, Board board) { ticksRemaining = 1; }
    public override void OnTick(Piece piece, Board board) { }
}
