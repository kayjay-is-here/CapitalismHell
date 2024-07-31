using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CapitalismHell.Common.Tools
{
    public class DebugInstance
    { 
        public bool ShowDebug { get; set; }

        public DebugInstance()
        {
            ShowDebug = false;
        }

        public void Log(string msg)
        {
            if (ShowDebug)
            {
                Main.NewText(msg, 255, 255, 204);
            }
        }
    }
}
