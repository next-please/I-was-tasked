using System;

namespace Com.Nextplease.IWT {
    [Serializable]
    public class PhaseManagementData : Data
    {
        public int numPlayers;
        public int round;
        
        public PhaseManagementData(int numPlayers, int round)
        {
            this.numPlayers = numPlayers;
            this.round = round;
        }

    }
}
