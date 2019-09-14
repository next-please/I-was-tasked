using System;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class PieceTransactionData : PieceData
    {
        public int price;

        public PieceTransactionData(Player player, Piece piece, int price) : base(player, piece)
        {
            this.price = price;
        }
    }
}
