using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CapitalismHell.Common.Systems
{
    public class InflationSystem : ModSystem
    {
        public float InflationRate { get; private set; }
        public const float BOSS_RATE_MULT = 1.5f;
        private HashSet<object> _bossesDefeated = new(); // @TODO determine object type

        public override void Load()
        {
            // Initialize or load data
            UpdateInflationRate();
        }

        public override void OnWorldLoad()
        {
            // Update inflation rate when the world is loaded
            UpdateInflationRate();
        }

        // @TODO Implement a hook listening to whenever a boss is defeated, by hooking onto the death function of an NPC and checking if it's a boss and adding its name to the list of bosses defeated.
        public void OnBossDefeat()
        {
            // Add the Boss defeated to a hashset
        }



        private void UpdateInflationRate()
        {
            InflationRate = 1.0f + _bossesDefeated.Count * BOSS_RATE_MULT; 
        }

        public int GetAmountAfterInflation(int amount) => (int)Math.Round((float)amount * InflationRate);
    }
}
