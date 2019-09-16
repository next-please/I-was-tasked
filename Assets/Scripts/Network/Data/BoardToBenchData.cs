using System;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class BoardToBenchData : Data
    {
        public Player player;
        public Piece piece;
        public int slotIndex;

        public BoardToBenchData(Player player, Piece piece, int slotIndex)
        {
            this.player = player;
            this.piece = piece;
            this.slotIndex = slotIndex;
        }
    }
}
