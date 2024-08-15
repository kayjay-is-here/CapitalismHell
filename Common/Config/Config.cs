using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace CapitalismHell.Common.Config
{

    public class CapitalismHellModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(0.2f)]
        public float StepBaseCost;

        [DefaultValue(1)]
        public int HurtBaseCost;

        // Add code for backwards compatibility here
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext ctx)
        {
            
        }

    }
}
