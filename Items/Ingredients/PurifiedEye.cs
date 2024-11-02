using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Sharingan.Buffs;
using Sharingan.Tiles.Furniture;

namespace Sharingan.Items.Ingredients
{
    internal class PurifiedEye : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 99;
         
            Item.rare = ItemRarityID.Purple;
          
        }

        public override void AddRecipes()
        {
            Recipe PurifiedEye = CreateRecipe();
            PurifiedEye.AddTile(ModContent.TileType<SharinganCraftingStation>());
            PurifiedEye.AddIngredient(ItemID.SuspiciousLookingEye, 1);
            PurifiedEye.AddIngredient(ItemID.Daybloom, 1);
            PurifiedEye.AddIngredient(ItemID.Waterleaf, 1);
            PurifiedEye.AddIngredient(ItemID.Blinkroot, 1);

            PurifiedEye.Register();
        }
    }
}
