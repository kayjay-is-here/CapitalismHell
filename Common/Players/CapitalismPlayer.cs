using CapitalismHell.Common.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CapitalismHell.Common.Players
{
    public class CapitalismPlayer : ModPlayer
    {
        public override void Load()
        {
            IL_Player.BuyItem += OnBuyItem;
        }

        private void OnBuyItem(ILContext IL)
        {

            try
            {
                ILCursor c = new(IL);

                c.GotoNext(i => i.MatchLdarg0()); // CanAfford(>>>this<<<, int64 price, int32 customCurrency)
                
                c.GotoNext(i => i.MatchLdarg0()); // PayCurrency(>>>this<<<, int64 price, int32 customCurrency)

                MethodInfo spawnMethod = typeof(CoinLossText).GetMethod("Spawn", BindingFlags.Public | BindingFlags.Instance);
                if (spawnMethod == null)
                {
                    throw new InvalidOperationException("Spawn method not found in CoinLossText.");
                }

                // Call custom function before loading in and calling PayCurrency
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.Emit(OpCodes.Call, spawnMethod); // Calls CoinLossText.Spawn(price)
            }
            catch (Exception e)
            {
                // If there are any failures with the IL editing, this method will dump the IL to Logs/ILDumps/{Mod Name}/{Method Name}.txt
                //MonoModHooks.DumpIL(ModContent.GetInstance<CapitalismHell>(), IL);

                // If the mod cannot run without the IL hook, throw an exception instead. The exception will call DumpIL internally
                throw new ILPatchFailureException(ModContent.GetInstance<CapitalismHell>(), IL, e);
            }
        }
    }
}
