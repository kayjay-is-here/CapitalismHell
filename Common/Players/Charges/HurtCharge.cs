using System;
using Terraria.ModLoader;
using Terraria;
using CapitalismHell.Common.Tools;
using CapitalismHell.Common.Systems;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.DataStructures;
using Terraria.Localization;
using CapitalismHell.Common.Config;

namespace CapitalismHell.Common.Players.Charges
{
    public class HurtCharge : ModPlayer
    {

        public DebugInstance Debug;
        public override void Initialize()
        {
            Debug = new();
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            int cost = ModContent.GetInstance<InflationSystem>().GetAmountAfterInflation(info.Damage * ModContent.GetInstance<CapitalismHellModConfig>().HurtBaseCost);
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
