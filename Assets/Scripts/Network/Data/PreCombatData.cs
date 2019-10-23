using System;
using System.Collections.Generic;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class PreCombatData : Data
    {
        public List<List<Piece>> enemies;
        public int randomIndex;

        public PreCombatData(List<List<Piece>> enemies, int randomIndex)
        {
            this.enemies = enemies;
            this.randomIndex = randomIndex;
        }

    }
}
