using System;
namespace Com.Nextplease.IWT
{
[Serializable]
public class InitPhaseData : Data
{
    public int numPlayers;
    public int seed;
    public InitPhaseData(int numPlayers, int seed)
    {
        this.numPlayers = numPlayers;
        this.seed = seed;
    }
}
}
