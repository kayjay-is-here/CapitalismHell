using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Common.Commands
{
    public class CoinsCommand : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command => "coins";

        // A short description of this command
        public override string Description => "Give the player one platinum coin";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Get the player who issued the command
            Player player = caller.Player;

            // Define the item ID for the platinum coin
            int platinumCoinItemID = ItemID.PlatinumCoin;

            // Define the quantity of platinum coins to be given (1 in this case)
            int quantity = 1;

            // Give the player the platinum coin
            player.QuickSpawnItem(player.GetSource_Misc("Command"), platinumCoinItemID, quantity);

            // Inform the player that the coin has been added
            Main.NewText("You have been given one platinum coin!", 255, 255, 0);
        }
    }
}
