using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ExampleMod.Common.Commands
{
    public class LoanCommand : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command => "getloan";

        // A short description of this command
        public override string Description => Language.GetTextValue("Mods.CapitalismHell.Commands.LoanCommand.Description");

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            // Get the player who issued the command
            Player player = caller.Player;

            // Define the item ID for the platinum coin
            int platinumCoinItemID = ItemID.GoldCoin;

            // Define the quantity of platinum coins to be given (1 in this case)
            int quantity = 10;

            // Give the player the platinum coin
            player.QuickSpawnItem(player.GetSource_Misc("Command"), platinumCoinItemID, quantity);

            // Inform the player that the coin has been added
            //Main.NewText("You have been given ten gold coins!", 255, 255, 0);
            Main.NewText(Language.GetTextValue("Mods.CapitalismHell.Commands.LoanCommand.ActionText"), 255, 255, 0);
        }
    }
}
