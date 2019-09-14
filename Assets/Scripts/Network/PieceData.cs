namespace Com.Nextplease.IWT
{
    public class PieceData : Data
    {
        public Player player;
        public Piece piece;

        public PieceData(Player player, Piece piece)
        {
            this.player = player;
            this.piece = piece;
        }
    }
}