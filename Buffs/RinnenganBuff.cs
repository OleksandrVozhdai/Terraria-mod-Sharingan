using Microsoft.Xna.Framework;
using Sharingan.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Sharingan.Buffs
{
    internal class RinnenganBuff : ModBuff
    {
        public static readonly int DefenseBonus = 20;

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += DefenseBonus;
            player.waterWalk = true;
          
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.25f); 

          
            player.statManaMax2 = (int)(player.statManaMax2 * 1.10f); 
            player.manaRegen += 2; 

         
            player.GetDamage(DamageClass.Generic) += 0.1f;

            player.statDefense += 40;
            player.manaRegen += 2; 

           
            player.lifeRegen += 2; 

        }
    }

    public class CustomNPCRenningan : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            Player player = Main.LocalPlayer;

       
            if (player.HasBuff(ModContent.BuffType<Buffs.RinnenganBuff>()))
            {
            
                if (Main.rand.NextBool(10)) 
                {
                 
                    Dust dust = Dust.NewDustPerfect(npc.position, DustID.PurpleTorch, new Vector2(0f, 0f), 150, Color.Blue, 1.5f);
                    dust.velocity *= 0.5f;
                    dust.noGravity = true;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            Player player = Main.LocalPlayer;

      
            if (player.HasBuff(ModContent.BuffType<RinnenganBuff>()))
            {
               
                drawColor = Color.MediumPurple; 

            }
        }
    }
}
