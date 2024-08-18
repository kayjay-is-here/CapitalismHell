using System;
using Terraria.ModLoader;
using Terraria;
using CapitalismHell.Common.Tools;
using Terraria.ModLoader.Exceptions;
using Terraria.Localization;
using Steamworks;
using CapitalismHell.Common.Config;

namespace CapitalismHell.Common.Players.Charges
{
    public class StepCharge : ModPlayer
    {
        public float LastX { get; set; }
        public float LastY { get; set;  }
        private int _counter;
        
        public DebugInstance Debug;

        public const float DISTANCE_EPSILON = 0.0001f; // Threshold for detecting movement
        public const int NUM_GRACE_TICKS = 5; // Number of ticks before starting to charge
        public const int NUM_CHARGE_COOLDOWN_TICKS = 5;
        public const int NUM_TEXT_COOLDOWN_TICKS = 50; // Cooldown for displaying messages

        public override void Initialize()
        {
            Debug = new();
        }

        public override void OnEnterWorld()
        {
            LastX = Player.position.X;
            LastY = Player.position.Y;
            _counter = 0;
        }

        public override void PostUpdate()
        {
            _counter++;

            // Skip charging if within intial grace period
            if (_counter < NUM_GRACE_TICKS)
                return;



            float currentX = Player.position.X;
            float currentY = Player.position.Y;

            bool hasMoved = Math.Abs(currentX - LastX) > DISTANCE_EPSILON || Math.Abs(currentY - LastY) > DISTANCE_EPSILON;

            // Check if the player is at the origin (0,0), and if so, update last position and skip charging
            if (IsAtOrigin(LastX, LastY))
            {
                UpdateLastPosition(currentX, currentY);
                return;
            }

            if (_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
            {
                Debug.Log($"DEBUG: currentPos=({currentX}, {currentY}) | lastPos=({LastX},{LastY}) | counter={_counter}");
            }

            // Charge the player for movement if applicable
            if (hasMoved)
            {
                ChargePlayerForStep(currentX, currentY);
            }

            // Reset the counter to prevent overflow
            if (_counter >= int.MaxValue)
            {
                _counter = NUM_GRACE_TICKS;
            }
        }

        // Check if the player's position is at the origin
        private static bool IsAtOrigin(float x, float y) => x < DISTANCE_EPSILON && y < DISTANCE_EPSILON;

        // Update the last known position of the player
        private void UpdateLastPosition(float x, float y)
        {
            LastX = x;
            LastY = y;
        }

        // Stop player movements
        private void StopMovement()
        {
            // Display a message if the player cannot afford to move, respecting the cooldown
            if (_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
            {
                Main.NewText(Language.GetTextValue("Mods.CapitalismHell.Charges.StepInsufficient"), 255, 0, 0);
            }

            // Prevent the player from moving by resetting their position
            Player.position.X = LastX; 
            Player.position.Y = LastY;
        }

        // Charge the player for taking a step
        private void ChargePlayerForStep(float currentX, float currentY)
        {
            bool isFalling = currentY - LastY > DISTANCE_EPSILON;
            double distanceMoved = Math.Sqrt(Math.Pow(currentX - LastX, 2) + Math.Pow(isFalling ? 0 : currentY - LastY, 2));
            int Cost = (int)Math.Ceiling(distanceMoved * ModContent.GetInstance<CapitalismHellModConfig>().StepBaseCost);
            Debug.Log($"Attempting to charge player {Cost}. Has moved (bool): {Player.position.X != LastX}");
            
            if(!isFalling && !Player.CanAfford(1))
            {
                StopMovement();
                return;
            }
            
            UpdateLastPosition(currentX, currentY);

            if (_counter % NUM_CHARGE_COOLDOWN_TICKS == 0)
            {
                if (Player.BuyItem(Cost))
                {
                    Debug.Log($"Charged for movement");
                }
            }
            else
            {
                StopMovement();
            }
        }
    }
}
