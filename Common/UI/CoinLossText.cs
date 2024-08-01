using CapitalismHell.Common.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;  // For Color and Vector2
using CapitalismHell.Common.Tools;
using Steamworks;

namespace CapitalismHell.Common.UI
{
    public static class CoinLossText
    {
        public const int COPPER_UNIT = 1;
        public const int SILVER_UNIT = COPPER_UNIT * 100;
        public const int GOLD_UNIT = SILVER_UNIT * 100;
        public const int PLATINUM_UNIT = GOLD_UNIT * 100;

        public static DebugInstance Debug = new();

        public static void Spawn(int price)
        {
            Debug.Log($"CoinLossText.cs: Spawn({price}) was ran");

            // Assuming the position for the text should be the player's position
            Player player = Main.LocalPlayer;
            Vector2 position = player.position;

            int coppers = (int)((float)price / COPPER_UNIT);
            int silvers = (int)((float)price / SILVER_UNIT);
            int golds = (int)((float)price / GOLD_UNIT);
            int platinums = (int)((float)price / PLATINUM_UNIT);

            // Spawn the combat text
            if (coppers > 0)
                CombatText.NewText(new Rectangle((int)position.X, (int)position.Y, player.width, player.height), new Color(150, 67, 22), $"-{coppers}", false, false);
            if (silvers > 0)
                CombatText.NewText(new Rectangle((int)position.X, (int)position.Y, player.width, player.height), Color.Gray, $"-{silvers}", false, false);
            if (golds > 0)
                CombatText.NewText(new Rectangle((int)position.X, (int)position.Y, player.width, player.height), Color.Gold, $"-{golds}", false, false);
            if (platinums > 0)
                CombatText.NewText(new Rectangle((int)position.X, (int)position.Y, player.width, player.height), Color.Silver, $"-{platinums}", false, false);

        }
    }
}
