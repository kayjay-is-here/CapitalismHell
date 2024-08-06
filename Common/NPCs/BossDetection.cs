using CapitalismHell.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CapitalismHell.Common.NPCs
{
    public class BossDetection : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            base.OnKill(npc);
            if (npc.boss)
            {
                ModContent.GetInstance<InflationSystem>().OnBossDefeat(npc);
            }
        }
    }
}
