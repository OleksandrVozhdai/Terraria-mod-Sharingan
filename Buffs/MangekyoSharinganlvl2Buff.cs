using Microsoft.Xna.Framework;
using Sharingan.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

    
namespace Sharingan.Buffs
{
    internal class MangekyoSharinganlvl2Buff :ModBuff
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


        }
    }

    public class CustomNPCMangekyoLvl2 : GlobalNPC
    {
        public override void AI(NPC npc)
        {
            Player player = Main.LocalPlayer;


            if (player.HasBuff(ModContent.BuffType<Buffs.MangekyoSharinganlvl2Buff>()))
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


            if (player.HasBuff(ModContent.BuffType<MangekyoSharinganlvl2Buff>()))
            {

                drawColor = Color.Red;

            }
        }


    }
}
