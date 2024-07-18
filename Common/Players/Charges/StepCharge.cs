using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using System.Drawing.Printing;
using System.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Metrics;

namespace CapitalismHell.Common.Players.Charges
{
 
    public class StepCharge : ModPlayer
    {
        private float _lastX;
        private float _lastY;
        private int _counter;
        public const bool SHOULD_CHARGE_FOR_FALL = false;
        public const int MOVE_COST = 1; // @TODO move this to config file
        public const float DISTANCE_EPSILON = 0.0001f;
        public const int NUM_GRACE_TICKS = 100;
        public const int NUM_TEXT_COOLDOWN_TICKS = 100;
        public override void Initialize()
        {
            _lastX = Player.position.X;
            _lastY = Player.position.Y;

        }

        public override void PostUpdate()
        {

            if (_counter < NUM_GRACE_TICKS)
            {
                _counter++;
                return;
            }
            
            float currentX = Player.position.X;
            float currentY = Player.position.Y;

            bool isFalling = currentY - _lastY > DISTANCE_EPSILON;
            bool hasMovedX = Math.Abs(currentX - _lastX) > DISTANCE_EPSILON;
            bool hasMovedY = Math.Abs(currentY - _lastY) > DISTANCE_EPSILON;
            bool isCoordAtOrigin = _lastX < DISTANCE_EPSILON && _lastY < DISTANCE_EPSILON;
            
            // Sometimes the game puts the player at the origin for a second and this shouldn't count as moving
            if(isCoordAtOrigin)
            {
                _lastX = currentX;
                _lastY = currentY;
                return;
            }

            if(_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
            {
                Main.NewText($"DEBUG: currentPos=({currentX}, {currentY}) | lastPos=({_lastX},{_lastY}) | counter={_counter}", 255,255,204);
            }
            
            if (hasMovedX)
                if (hasMovedY && !(isFalling && SHOULD_CHARGE_FOR_FALL))
                    ChargePlayerForStep(currentX, currentY);
               

        }

        private void ChargePlayerForStep(float currentX, float currentY)
        {
            if(Player.CanAfford(MOVE_COST))
            {
                Player.BuyItem(MOVE_COST);
                _lastX = currentX;
                _lastY = currentY;
            }
            else
            {
                // @TODO introduce cooldown for this text appearing

                if(_counter % NUM_TEXT_COOLDOWN_TICKS == 0)
                    Main.NewText("You do not currently have enough money to move!");
                
                Player.position.X = _lastX;
                Player.position.Y = _lastY;
                //Main.NewText("Error, cannot halt player. Reason: NotImplementedException", 255, 0, 0);
            }
        }
    }
}
