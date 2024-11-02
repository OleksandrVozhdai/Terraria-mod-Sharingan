using Sharingan.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sharingan.Items.Potions
{
    internal class SakuraPills : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 15;
            Item.healLife = 300;
            Item.potion = true;
            Item.useTurn = true;
            Item.buffType = BuffID.Regeneration;
            Item.buffType = ModContent.BuffType<DisgustingBuff>();
            
            Item.buffTime = 3600;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item2;
            
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffID.Confused, 300);
        }

        public override void AddRecipes()
        {
            base.AddRecipes();

            Recipe SakuraPills = CreateRecipe();
            SakuraPills.AddTile(TileID.Bottles);
            SakuraPills.AddIngredient(ItemID.Daybloom, 1 );
            SakuraPills.AddIngredient(ItemID.Moonglow, 1);
            SakuraPills.AddIngredient(ItemID.Blinkroot, 1);
            SakuraPills.AddIngredient(ItemID.Deathweed, 1);
            SakuraPills.AddIngredient(ItemID.Waterleaf, 1);
            SakuraPills.AddIngredient(ItemID.Fireblossom, 1);
            SakuraPills.Register();
        }


    }
}
