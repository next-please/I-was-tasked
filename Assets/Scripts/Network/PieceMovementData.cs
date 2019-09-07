namespace Com.Nextplease.IWT
{
    public class PieceMovementData : Data
    {
        public Player player;
        public Piece piece;
        public Tile tile;

        public PieceMovementData(Player player, Piece piece, Tile tile)
        {
            this.player = player;
            this.piece = piece;
            this.tile = tile;
        }

    }
}
