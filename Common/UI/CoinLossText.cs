using CapitalismHell.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using CapitalismHell.Common.Tools;

namespace CapitalismHell.Common.UI
{
    public class CoinLossText : CombatText
    {
        public static DebugInstance Debug = new();

        public CoinLossText()
        {
        }
            
        public void Spawn(int price)
        {
            Debug.Log($"CoinLessText.cs: Spawn({price}) was ran");
        }
    }
}
