using System;


namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeMarketSizeData : Data
{
    public Player player;
    public Piece piece;
    public UpgradeMarketSizeData(Player player, Piece piece)
    {
        this.player = player;
        this.piece = piece;
    }
}
}
