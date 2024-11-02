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
    internal class PushDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
         
            Main.debuff[Type] = true; 
            Main.buffNoSave[Type] = true; 
        }
        public override void Update(Player player, ref int buffIndex)
        {
         
        }

    }
}
