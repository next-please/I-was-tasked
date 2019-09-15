using System;

namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeArmySizeData : Data
{
    public Player player;
    public UpgradeArmySizeData(Player player)
    {
        this.player = player;
    }
}
}
