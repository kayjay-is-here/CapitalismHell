//using ExampleMod.Common.UI.ExampleCoinsUI;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ReLogic.Content;
//using System;
//using System.Collections.Generic;
//using Terraria;
//using Terraria.Audio;
//using Terraria.GameContent;
//using Terraria.ID;
//using Terraria.Localization;
//using Terraria.ModLoader;
//using Terraria.UI;

//namespace CapitalismHell.Common.UI.CoinStatsUI
//{
//    internal class CoinStatsUIState : UIState
//    {
//        public const bool DebugMode = true; // Toggle debug output
//        public CoinStatsDraggableUIPanel CoinCounterPanel;
//        public UIMoneyDisplay MoneyDisplay;

//        public override void OnInitialize()
//        {
//            /* Here we define our container UIElement. 
//             * In DraggableUIPanel.cs, you can see that 
//             * DraggableUIPanel is a UIPanel with added features. */
//            CoinCounterPanel = new CoinStatsDraggableUIPanel();
//            CoinCounterPanel.SetPadding(0);
//            SetRectangle(CoinCounterPanel, left: 400f, top: 100f, width: 250f, height: 100f);
//            CoinCounterPanel.BackgroundColor = new Color(73, 94, 171);

//            /* Next, we create another UIElement that we will place. 
//             * Since we will be calling `coinCounterPanel.Append(playButton);`, 
//             * Left and Top are relative to the top left of the coinCounterPanel UIElement. */
//            Asset<Texture2D> buttonPlayTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay");
//            CoinStatsUIHoverImageButton playButton = new(buttonPlayTexture, "Reset Coins Counters");
//            SetRectangle(playButton, left: 200f, top: 10f, width: 22f, height: 22f);
//            playButton.OnLeftClick += new MouseEvent(PlayButtonClicked);
//            CoinCounterPanel.Append(playButton);

//            /* Add a close button */
//            Asset<Texture2D> buttonDeleteTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete");
//            CoinStatsUIHoverImageButton closeButton = new(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52")); // Localized text for "Close"
//            SetRectangle(closeButton, left: 230f, top: 10f, width: 22f, height: 22f);
//            closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
//            CoinCounterPanel.Append(closeButton);

//            /* Add money display element */
//            MoneyDisplay = new UIMoneyDisplay();
//            SetRectangle(MoneyDisplay, 15f, 20f, 220f, 60f);
//            CoinCounterPanel.Append(MoneyDisplay);

//            // Append main panel to the UI state
//            Append(CoinCounterPanel);
//        }

//        /* Helper method to set the position and size of UI elements */
//        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
//        {
//            uiElement.Left.Set(left, 0f);
//            uiElement.Top.Set(top, 0f);
//            uiElement.Width.Set(width, 0f);
//            uiElement.Height.Set(height, 0f);
//        }

//        /* Handle the reset button click event */
//        private void PlayButtonClicked(UIMouseEvent evt, UIElement listeningElement)
//        {
//            SoundEngine.PlaySound(SoundID.MenuOpen);
//            MoneyDisplay.ResetCoins();
//            if (DebugMode) Main.NewText("Coins reset.", new Color(255, 255, 204));
//        }

//        /* Handle the close button click event */
//        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
//        {
//            SoundEngine.PlaySound(SoundID.MenuClose);
//            ModContent.GetInstance<CoinStatsUISystem>().HideMyUI();
//            if (DebugMode) Main.NewText("UI closed.", new Color(255, 255, 204));
//        }

//        /* Update the coin value displayed */
//        public void UpdateValue(int pickedUp)
//        {
//            MoneyDisplay.AddCoins(pickedUp);
//            if (DebugMode) Main.NewText($"Updated Value: {pickedUp}, Total Collected Coins: {MoneyDisplay.collectedCoins}", new Color(255, 255, 204));
//        }
//    }

//    public class UIMoneyDisplay : UIElement
//    {
//        public long collectedCoins; /* Total collected coins */
//        private List<(DateTime time, int amount)> coinChanges; /* List of coin changes with timestamps */
//        private readonly Texture2D[] _coinsTextures = new Texture2D[4]; /* Coin textures */

//        public UIMoneyDisplay()
//        {
//            coinChanges = new List<(DateTime time, int amount)>();

//            /* Load coin textures */
//            for (int j = 0; j < 4; j++)
//            {
//                Main.instance.LoadItem(74 - j);
//                _coinsTextures[j] = TextureAssets.Item[74 - j].Value;
//            }
//        }

//        /* Add coins */
//        public void AddCoins(int coins)
//        {
//            collectedCoins += coins;
//            coinChanges.Add((DateTime.Now, coins));
//            if (CoinStatsUIState.DebugMode) Main.NewText($"Coins added: {coins}, Total: {collectedCoins}", new Color(255, 255, 204));
//        }

//        /* Calculate coins per second */
//        public int GetCoinsPerSecond()
//        {
//            DateTime oneSecondAgo = DateTime.Now.AddSeconds(-1);
//            int coinsPerSecond = 0;

//            for (int i = coinChanges.Count - 1; i >= 0; i--)
//            {
//                if (coinChanges[i].time < oneSecondAgo)
//                {
//                    break;
//                }
//                coinsPerSecond += coinChanges[i].amount;
//            }

//            if (CoinStatsUIState.DebugMode) Main.NewText($"Coins per second: {coinsPerSecond}", new Color(255, 255, 204));
//            return coinsPerSecond;
//        }

//        /* Calculate coins per minute */
//        public int GetCoinsPerMinute()
//        {
//            DateTime oneMinuteAgo = DateTime.Now.AddMinutes(-1);
//            int coinsPerMinute = 0;

//            for (int i = coinChanges.Count - 1; i >= 0; i--)
//            {
//                if (coinChanges[i].time < oneMinuteAgo)
//                {
//                    break;
//                }
//                coinsPerMinute += coinChanges[i].amount;
//            }

//            if (CoinStatsUIState.DebugMode) Main.NewText($"Coins per minute: {coinsPerMinute}", new Color(255, 255, 204));
//            return coinsPerMinute;
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            CalculatedStyle innerDimensions = GetInnerDimensions();
//            float shopx = innerDimensions.X;
//            float shopy = innerDimensions.Y;

//            int cps = GetCoinsPerSecond();
//            int cpm = GetCoinsPerMinute();

//            // Draw coins per second
//            DrawCoins(spriteBatch, shopx, shopy, Utils.CoinsSplit(cps), 0, 0);
//            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "CPS", shopx + (float)(24 * 4), shopy, Color.White, Color.Black, new Vector2(0.3f), 0.75f);

//            // Draw coins per minute
//            DrawCoins(spriteBatch, shopx, shopy, Utils.CoinsSplit(cpm), 0, 25);
//            Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "CPM", shopx + (float)(24 * 4), shopy + 25f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
//        }

//        /* Draw the coins on the UI */
//        private void DrawCoins(SpriteBatch spriteBatch, float shopx, float shopy, int[] coinsArray, int xOffset = 0, int yOffset = 0)
//        {
//            for (int j = 0; j < 4; j++)
//            {
//                spriteBatch.Draw(_coinsTextures[j], new Vector2(shopx + 11f + 24 * j + xOffset, shopy + yOffset), null, Color.White, 0f, _coinsTextures[j].Size() / 2f, 1f, SpriteEffects.None, 0f);
//                string coinText = coinsArray[3 - j].ToString();
//                if (coinsArray[3 - j] < 0)
//                {
//                    coinText = $"({Math.Abs(coinsArray[3 - j])})"; // Optional: Display negative values in parentheses
//                }
//                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, coinText, shopx + 24 * j + xOffset, shopy + yOffset, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
//            }
//        }

//        /* Reset the coin count and timer */
//        public void ResetCoins()
//        {
//            collectedCoins = 0;
//            coinChanges.Clear();
//            if (CoinStatsUIState.DebugMode) Main.NewText("Coins and changes reset.", new Color(255, 255, 204));
//        }
//    }

//    public class MoneyCounterGlobalItem : GlobalItem
//    {
//        /* Applies to all types of coins */
//        public override bool AppliesToEntity(Item item, bool lateInstantiation)
//        {
//            return item.type >= ItemID.CopperCoin && item.type <= ItemID.PlatinumCoin;
//        }

//        /* Update UI on coin pickup */
//        public override bool OnPickup(Item item, Player player)
//        {
//            bool result = base.OnPickup(item, player);

//            int coinsAdded = item.stack * (item.value / 5);
//            ModContent.GetInstance<CoinStatsUISystem>()._coinStatsUI.UpdateValue(coinsAdded);
//            if (CoinStatsUIState.DebugMode) Main.NewText($"Coins picked up: {coinsAdded}", new Color(255, 255, 204));

//            return result;
//        }

//        /* Update UI on coin consumption */
//        public override void OnConsumeItem(Item item, Player player)
//        {
//            base.OnConsumeItem(item, player);

//            if (item.type >= ItemID.CopperCoin && item.type <= ItemID.PlatinumCoin)
//            {
//                int coinsConsumed = -item.stack * (item.value / 5);
//                ModContent.GetInstance<CoinStatsUISystem>()._coinStatsUI.UpdateValue(coinsConsumed);
//                if (CoinStatsUIState.DebugMode) Main.NewText($"Coins consumed: {coinsConsumed}", new Color(255, 255, 204));
//            }
//        }
//    }
//}
