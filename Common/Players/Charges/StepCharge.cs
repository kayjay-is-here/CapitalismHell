using System;
using Terraria.ModLoader;
using Terraria;
using CapitalismHell.Common.Tools;

namespace CapitalismHell.Common.Players.Charges
{
    public class StepCharge : ModPlayer
    {
        private float _lastX;
        private float _lastY;
        private int _counter;
        
        public DebugInstance Debug;

        public const double COST_MULT = 0.2; // Cost per move, should be moved to config
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
            _lastX = Player.position.X;
            _lastY = Player.position.Y;
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

            bool hasMoved = Math.Abs(currentX - _lastX) > DISTANCE_EPSILON || Math.Abs(currentY - _lastY) > DISTANCE_EPSILON;

            // Check if the player is at the origin (0,0), and if so, update last position and skip charging
            if (IsAtOrigin(_lastX, _lastY))
            {
                UpdateLastPosition(currentX, currentY);
                return;
            }

            if (_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
            {
                Debug.Log($"DEBUG: currentPos=({currentX}, {currentY}) | lastPos=({_lastX},{_lastY}) | counter={_counter}");
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
            _lastX = x;
            _lastY = y;
        }

        // Stop player movements
        private void StopMovement()
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

        // Charge the player for taking a step
        private void ChargePlayerForStep(float currentX, float currentY)
        {
            bool isFalling = currentY - _lastY > DISTANCE_EPSILON;

            double CostBeforeMultiplier = Math.Abs(currentX - _lastX) + (int)Math.Abs(isFalling ? 0 : currentY - _lastY);
            int Cost = (int)Math.Ceiling(CostBeforeMultiplier * COST_MULT);
            Debug.Log($"DEBUG: Attempting to charge player {Cost}. Has moved (bool): {Player.position.X != _lastX}");
            
            if(!Player.CanAfford(1))
            {
                StopMovement();
                return;
            }
            
            UpdateLastPosition(currentX, currentY);

            if (_counter % NUM_CHARGE_COOLDOWN_TICKS == 0)
            {
                if (Player.BuyItem(Cost))
                {
                    Debug.Log($"DEBUG: Charged for movement");
                }
            }
            else
            {
                StopMovement();
            }
        }
    }
}
