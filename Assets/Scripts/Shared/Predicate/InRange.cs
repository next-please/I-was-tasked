public class InRange : Predicate
{
  public bool IsTrue(Piece piece, Board board)
    {
        Piece target = piece.GetTarget();
        Tile targetTile = target.GetCurrentTile();
        return (piece.GetCurrentTile().DistanceToTile(targetTile) <= piece.GetAttackRange());
    }
}
