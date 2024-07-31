using CapitalismHell.Common.Players.Charges;
using CapitalismHell.Common.UI;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CapitalismHell.Common.Commands
{
    public class CapDebug : ModCommand
    {
        // CommandType.Chat means that command can be used in Chat in SP and MP
        public override CommandType Type => CommandType.Chat;

        // The desired text to trigger this command
        public override string Command => "capdebug";

        // A short description of this command
        public override string Description => "Toggles debug info for the Capitalism Hell mod";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            if (args == null || args.Length == 0)
            {
                Main.NewText($"You must specify what feature's debug you want to toggle.", 255, 0, 0);
                return;
            }

            // @TODO perhaps optimise this in the future
            switch (args[0])
            {
                case "stepCharge":
                    StepCharge stepCharge = caller.Player.GetModPlayer<StepCharge>();
                    stepCharge.Debug.ShowDebug = !stepCharge.Debug.ShowDebug;
                    Main.NewText($"stepCharge instance's debug messages have been {(stepCharge.Debug.ShowDebug ? "enabled" : "disabled")}", 255, 255, 204);
                    break;
                case "coinLossText":    
                    CoinLossText.Debug.ShowDebug = !CoinLossText.Debug.ShowDebug;
                    Main.NewText($"CoinLossText's debug messages have been {(CoinLossText.Debug.ShowDebug ? "enabled" : "disabled")}", 255, 255, 204);
                    break;
                default:
                    Main.NewText($"\"{args[0]}\" is not part of any known feature.", 255, 255, 204);
                    break;
            }
        }
    }
}
