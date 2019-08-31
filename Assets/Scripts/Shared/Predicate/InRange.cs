public class InRange : Predicate
{
  public bool IsTrue(Piece piece)
    {
        return piece.CanAttackTarget();
    }
}
