using System;


namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeMarketSizeData : Data
{
    public Player player;
    public UpgradeMarketSizeData(Player player)
    {
        this.player = player;
    }
}
}
