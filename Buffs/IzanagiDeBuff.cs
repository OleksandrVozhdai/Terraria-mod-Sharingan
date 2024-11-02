using Microsoft.Xna.Framework;
using Sharingan.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Sharingan.Buffs
{
    internal class IzanagiDeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
         
            Main.debuff[Type] = true; // Indicates that this is a debuff
            Main.buffNoSave[Type] = true; // Does not save this buff between sessions
            
        }
        public override void Update(Player player, ref int buffIndex)
        {
         
        }

    }
}
