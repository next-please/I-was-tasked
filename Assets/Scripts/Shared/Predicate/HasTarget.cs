public class HasTarget : Predicate
{
    public bool IsTrue(Piece piece, Board board)
    {
        Piece target = piece.GetTarget();
        return (target != null && !target.IsDead());
    }
}
