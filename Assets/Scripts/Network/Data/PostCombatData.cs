using System;

namespace Com.Nextplease.IWT
{
    [Serializable]
    public class PostCombatData : Data
    {
        public int health;
        public int[] gold;

        public PostCombatData(int health, int[] gold)
        {
            this.health = health;
            this.gold = gold;
        }
    }
}
