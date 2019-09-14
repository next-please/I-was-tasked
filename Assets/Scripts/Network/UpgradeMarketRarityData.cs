using System;
using System.Collections.Generic;

namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeMarketRarityData : Data
{
    public Player player;
    public List<Piece> pieces;
    public UpgradeMarketRarityData(Player player, List<Piece> pieces)
    {
        this.player = player;
        this.pieces = pieces;
    }
}
}
