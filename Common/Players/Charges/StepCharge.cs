using System;
using Terraria.ModLoader;
using Terraria;

namespace CapitalismHell.Common.Players.Charges
{
    public class StepCharge : ModPlayer
    {
        private float _lastX;
        private float _lastY;
        private int _counter;

        public bool Debug { get; set; } = true; // Toggle for debug messages
        public const bool SHOULD_CHARGE_FOR_FALL = false; // Flag to charge for falling
        public const int MOVE_COST = 1; // Cost per move, should be moved to config
        public const float DISTANCE_EPSILON = 0.0001f; // Threshold for detecting movement
        public const int NUM_GRACE_TICKS = 5; // Number of ticks before starting to charge
        public const int NUM_TEXT_COOLDOWN_TICKS = 50; // Cooldown for displaying messages

        public override void Initialize()
        {
            _lastX = Player.position.X;
            _lastY = Player.position.Y;
            _counter = 0;
        }

        public override void PostUpdate()
        {
            _counter++;

            // Skip charging if within grace period
            if (_counter < NUM_GRACE_TICKS)
                return;

            float currentX = Player.position.X;
            float currentY = Player.position.Y;

            bool isFalling = currentY - _lastY > DISTANCE_EPSILON;
            bool hasMoved = Math.Abs(currentX - _lastX) > DISTANCE_EPSILON || Math.Abs(currentY - _lastY) > DISTANCE_EPSILON;

            // Check if the player is at the origin (0,0), and if so, update last position and skip charging
            if (IsAtOrigin(_lastX, _lastY))
            {
                UpdateLastPosition(currentX, currentY);
                return;
            }

            // Log debug information if applicable
            LogDebug(currentX, currentY);

            // Charge the player for movement if applicable
            //if (hasMoved && !(isFalling && !SHOULD_CHARGE_FOR_FALL))
            if(hasMoved)
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
            _lastX = x;
            _lastY = y;
        }

        // Log debug information if debug is enabled
        private void LogDebug(float currentX, float currentY)
        {
            if (Debug)
            {
                if (_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
                {
                    Main.NewText($"DEBUG: currentPos=({currentX}, {currentY}) | lastPos=({_lastX},{_lastY}) | counter={_counter}", 255, 255, 204);
                }
            }
        }

        // Charge the player for taking a step
        private void ChargePlayerForStep(float currentX, float currentY)
        {
            LogDebugInfo($"DEBUG: Attempting to charge player. Has moved: {Player.position.X != _lastX}");
            if (Player.BuyItem(MOVE_COST))
            {
                LogDebugInfo($"DEBUG: Charged {MOVE_COST} for movement");
                UpdateLastPosition(currentX, currentY);
            }
            else
            {
                // Display a message if the player cannot afford to move, respecting the cooldown
                if (_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
                {
                    Main.NewText("You do not currently have enough money to move!", 255, 0, 0);
                }

                // Prevent the player from moving by resetting their position
                Player.position.X = _lastX;
                Player.position.Y = _lastY;
            }
        }

        // Log debug information
        private void LogDebugInfo(string message)
        {
            if (Debug)
            {
                Main.NewText(message, 255, 255, 204);
            }
        }
    }
}
