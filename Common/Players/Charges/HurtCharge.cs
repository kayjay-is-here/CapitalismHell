using System;
using Terraria.ModLoader;
using Terraria;
using CapitalismHell.Common.Tools;
using CapitalismHell.Common.Systems;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.DataStructures;
using Terraria.Localization;

namespace CapitalismHell.Common.Players.Charges
{
    public class HurtCharge : ModPlayer
    {
        public const int COST_MULT = 1;

        public DebugInstance Debug;
        public override void Initialize()
        {
            Debug = new();
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            int cost = ModContent.GetInstance<InflationSystem>().GetAmountAfterInflation(info.Damage * COST_MULT);
            if (!Player.BuyItem(cost))
            {
                LocalizedText deathMessage = Language.GetText("Mods.CapitalismHell.Charges.HurtInsufficient.DeathMessage");
                Player.KillMe(PlayerDeathReason.ByCustomReason(deathMessage.Format(Player.name)), double.MaxValue, 0);
            }
            else
            {
                Debug.Log($"Charged player {cost} for being damaged by {info.Damage} points.");
            }
        }
    }
}
