public class HasTarget : Predicate
{
    public bool IsTrue(Piece piece)
    {
        Piece target = piece.GetTarget();
        return (target != null && !target.IsDead());
    }
}
