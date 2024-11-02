using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sharingan.Buffs
{
    internal class DisgustingBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            base.Unload();
            player.manaRegenBuff = true;
           
        }
    }
}
