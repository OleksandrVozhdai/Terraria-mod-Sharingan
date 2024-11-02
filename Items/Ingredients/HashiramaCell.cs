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
    internal class HashiramaCell : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 99;
         
            Item.rare = ItemRarityID.Purple;
          
        }

        public override void AddRecipes()
        {
            Recipe HashiramaCell = CreateRecipe();
            HashiramaCell.AddTile(ModContent.TileType<SharinganCraftingStation>());
            HashiramaCell.AddIngredient(ItemID.FragmentNebula, 3);
            HashiramaCell.AddIngredient(ItemID.FragmentSolar, 3);
            HashiramaCell.AddIngredient(ItemID.FragmentStardust, 3);
            HashiramaCell.AddIngredient(ItemID.FragmentVortex, 3);
            HashiramaCell.AddIngredient(ItemID.LunarBar, 1);
            HashiramaCell.Register();
        }
    }
}
