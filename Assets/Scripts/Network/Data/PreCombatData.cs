using System;
using System.Collections.Generic;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class PreCombatData : Data
    {
        public List<List<Piece>> enemies;

        public PreCombatData(List<List<Piece>> enemies)
        {
            this.enemies = enemies;
        }

    }
}
