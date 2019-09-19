using System;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class InitPhaseData : Data
    {
        public int numPlayers;
        public int[] seeds;

        public InitPhaseData(int numPlayers, int[] seeds)
        {
            this.numPlayers = numPlayers;
            this.seeds = new int[seeds.Length];
            for (int i = 0; i < seeds.Length; i++)
            {
                this.seeds[i] = seeds[i];
            }
        }
    }
}
