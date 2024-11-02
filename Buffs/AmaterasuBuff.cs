using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Sharingan.Buffs
{
    internal class AmaterasuBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.active && !npc.friendly)
            {
               
                npc.onFire3 = true;

               
                int dustType = ModContent.DustType<AmaterasuDust>();
                for (int i = 0; i < 5; i++)
                {
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].scale = 1.5f;
                }
            }
        }
    }

    public class AmaterasuDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.color = Color.Black; 
            dust.noGravity = true; 
            dust.scale = 1.5f; 
        }

        public override bool Update(Dust dust)
        {
            dust.alpha += 5; 
            if (dust.alpha >= 255)
            {
                dust.active = false;
            }
            return true;
        }
    }
}
