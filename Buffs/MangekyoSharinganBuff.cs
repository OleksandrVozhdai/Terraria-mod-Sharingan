using Microsoft.Xna.Framework;
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
    internal class MangekyoSharinganBuff : ModBuff
    {
        public static readonly int DefenseBonus = 15;

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
           
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f; 
            player.statDefense += DefenseBonus;
            player.waterWalk = true;
            player.detectCreature = true;
          
           
        }
    }

    public class CustomNPCMangekyo : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            Player player = Main.LocalPlayer;

          
            if (player.HasBuff(ModContent.BuffType<Buffs.MangekyoSharinganBuff>()))
            {
                
                if (Main.rand.NextBool(10)) 
                {
                 
                    Dust dust = Dust.NewDustPerfect(npc.position, DustID.RedStarfish, new Vector2(0f, 0f), 150, Color.Blue, 1.5f);
                    dust.velocity *= 0.5f; 
                    dust.noGravity = true;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            Player player = Main.LocalPlayer;

         
            if (player.HasBuff(ModContent.BuffType<MangekyoSharinganBuff>()))
            {
              
                drawColor = Color.Red; 

            }
        }

        
    }


}
