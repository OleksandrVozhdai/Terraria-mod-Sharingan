using Microsoft.Xna.Framework;
using Sharingan.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Sharingan.Buffs
{
    internal class TpCooldownBuff : ModBuff
    {
        public static readonly int DefenseBonus = 10;

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += DefenseBonus;
            player.waterWalk = true;


        }
    }
}
