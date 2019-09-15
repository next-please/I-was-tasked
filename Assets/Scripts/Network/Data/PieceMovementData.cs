using System;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class PieceMovementData : PieceData
    {
        public Tile tile;

        public PieceMovementData(Player player, Piece piece, Tile tile) : base(player, piece)
        {
            this.tile = tile;
        }
    }
}
