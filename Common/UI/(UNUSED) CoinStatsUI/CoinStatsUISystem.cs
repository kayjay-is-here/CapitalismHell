//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Terraria;
//using Terraria.ModLoader;
//using Terraria.UI;

//namespace CapitalismHell.Common.UI.CoinStatsUI
//{
//	[Autoload(Side = ModSide.Client)] // This attribute makes this class only load on a particular side. Naturally this makes sense here since UI should only be a thing clientside. Be wary though that accessing this class serverside will error
//	public class CoinStatsUISystem : ModSystem
//	{
//		private UserInterface _coinStatsUserInterface;
//		internal CoinStatsUIState _coinStatsUI;

//		// These two methods will set the state of our custom UI, causing it to show or hide
//		public void ShowMyUI() {
//			_coinStatsUserInterface?.SetState(_coinStatsUI);
//		}

//		public void HideMyUI() {
//			_coinStatsUserInterface?.SetState(null);
//		}

//		public override void Load() {
//			if (!Main.dedServ)
//			{
//				// Create custom interface which can swap between different UIStates
//				_coinStatsUserInterface = new UserInterface();
//				// Creating custom UIState
//				_coinStatsUI = new CoinStatsUIState();

//				// Activate calls Initialize() on the UIState if not initialized, then calls OnActivate and then calls Activate on every child element
//				_coinStatsUI.Activate();
//			}
//		}

//		public override void UpdateUI(GameTime gameTime) {
//			// Here we call .Update on our custom UI and propagate it to its state and underlying elements
//			if (_coinStatsUserInterface?.CurrentState != null) {
//				_coinStatsUserInterface?.Update(gameTime);
//			}
//		}

//		// Adding a custom layer to the vanilla layer list that will call .Draw on your interface if it has a state
//		// Setting the InterfaceScaleType to UI for appropriate UI scaling
//		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
//			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
//			if (mouseTextIndex != -1) {
//				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
//					"Capitalism Hell Mod: Coins Changed Per Second",
//					delegate {
//						if (_coinStatsUserInterface?.CurrentState != null) {
//							_coinStatsUserInterface.Draw(Main.spriteBatch, new GameTime());
//						}
//						return true;
//					},
//					InterfaceScaleType.UI)
//				);
//			}
//		}
//	}
//}
