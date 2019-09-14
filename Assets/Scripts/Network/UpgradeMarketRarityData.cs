using System;

namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeMarketRarityData : Data
{
    public Player player;
    public UpgradeMarketRarityData(Player player)
    {
        this.player = player;
    }
}
}
