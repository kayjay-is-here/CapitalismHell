using System;
using Terraria;
using Terraria.ModLoader;

namespace CapitalismHell.Common.Systems
{
    public class InflationSystem : ModSystem
    {
        public float InflationRate { get; set; }
        public const float BOSS_RATE_MULT = 1.5f;
        private int _bossDefeatedCount; 

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

        // @TODO Implement a hook listening to whenever a boss is defeated



        private void UpdateInflationRate()
        {
            InflationRate = 1.0f + (_bossDefeatedCount <= 0 ? 1 : _bossDefeatedCount * BOSS_RATE_MULT);
        }

        public int GetAmountAfterInflation(int amount) => (int)Math.Round((float)amount * InflationRate);
    }
}
