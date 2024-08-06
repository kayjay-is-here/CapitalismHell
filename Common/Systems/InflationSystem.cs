using CapitalismHell.Common.Tools;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CapitalismHell.Common.Systems
{
    public class InflationSystem : ModSystem
    {
        public float InflationRate { get; private set; }
        public static DebugInstance Debug;
        public const float BOSS_RATE_MULT = 1.5f;
        private HashSet<string> _bossesDefeated = [];
        //private List<NPC> _bossesDefeated = [];
        public override void OnWorldLoad()
        {
            // Update inflation rate when the world is loaded
            UpdateInflationRate();
        }

        // @TODO Implement a hook listening to whenever a boss is defeated, by hooking onto the death function of an NPC and checking if it's a boss and adding its name to the list of bosses defeated.
        public void OnBossDefeat(NPC boss)
        {
            // Add the Boss defeated to a hashset
            if (!_bossesDefeated.Add(boss.FullName))
                return;

            Debug.Log($"{boss.FullName} has been added to the boss defeated list.");
            Debug.Log($"New boss count: {_bossesDefeated.Count}");

            // Initialize or load data
            Debug.Log($"Original InflationRate: {InflationRate}");
            
            UpdateInflationRate();

            Debug.Log($"New InflationRate: {InflationRate}");

        }

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> arr;
            arr = _bossesDefeated.ToList();
            tag.Add("bossesDefeated", arr);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            tag.TryGet("bossesDefeated", out List<string> arr);
            _bossesDefeated = new(arr);

        }

        private void UpdateInflationRate()
        {
            InflationRate = 1.0f + _bossesDefeated.Count * BOSS_RATE_MULT; 
        }

        public int GetAmountAfterInflation(int amount) => (int)Math.Round(amount * InflationRate);
    }
}
