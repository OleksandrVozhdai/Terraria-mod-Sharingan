using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace Sharingan.Buffs
{
    public class GenjutsuSharingan : ModBuff
    {
        public override void SetStaticDefaults()
        {
          
        }


        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.localAI[0] > 0) 
            {
              //  npc.noTileCollide = true;

                npc.velocity = new Vector2(0.01f, npc.velocity.Y);

                npc.localAI[0]--; 
            }
            else
            {
                npc.DelBuff(buffIndex); 
                buffIndex--; 
            }
        }

    }

}
