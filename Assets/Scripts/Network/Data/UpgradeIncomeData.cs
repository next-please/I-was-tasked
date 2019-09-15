using System;

namespace Com.Nextplease.IWT
{
[Serializable]
public class UpgradeIncomeData : Data
{
    public Player player;
    public UpgradeIncomeData(Player player)
    {
        this.player = player;
    }
}
}
