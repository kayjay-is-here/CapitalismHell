using ExampleMod.Common.Systems;
using ExampleMod.Common.UI.ExampleCoinsUI;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Common.Players
{
	// See Common/Systems/KeybindSystem for keybind registration.
	public class CoinDisplayKeybindPlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (KeybindSystem.CoinDisplayKeybind.JustPressed) {
                ModContent.GetInstance<ExampleCoinsUISystem>().ShowMyUI();
                Main.NewText($"Capitalism Mod's CoinDisplayKeybind was just pressed.");
			}
		}
	}
}
