using System;
using System.Collections.Generic;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class MarketManagementData : PhaseManagementData
    {
        public List<Piece> pieces;

        public MarketManagementData(int numPlayers, int round, List<Piece> pieces) : base(numPlayers, round)
        {
            this.pieces = pieces;
        }
    }
}
