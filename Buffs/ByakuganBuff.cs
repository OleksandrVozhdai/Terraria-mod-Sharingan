using Microsoft.Xna.Framework;
using Sharingan.Items;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;


namespace Sharingan.Buffs
{
    internal class ByakuganBuff : ModBuff
    {
        public static readonly int DefenseBonus = 10;

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += DefenseBonus;
            player.waterWalk = true;
            player.nightVision = true;
            Lighting.AddLight(player.position, 4f, 4f, 4f);
            player.dangerSense = true;
            player.GetDamage(DamageClass.Magic) += 0.2f;
            player.findTreasure = true;
           
        }
    }

    public class CustomNPC : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            Player player = Main.LocalPlayer;

          
            if (player.HasBuff(ModContent.BuffType<Buffs.ByakuganBuff>()))
            {
                if (Main.rand.NextBool(10)) 
                {
                   
                    Dust dust = Dust.NewDustPerfect(npc.position, DustID.BlueCrystalShard, new Vector2(0f, 0f), 150, Color.Blue, 1.5f);
                    dust.velocity *= 0.5f; 
                    dust.noGravity = true; 
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            Player player = Main.LocalPlayer;

           
            if (player.HasBuff(ModContent.BuffType<ByakuganBuff>()))
            {
            
                drawColor = Color.Cyan; 
                
            }
        }
    }
}
