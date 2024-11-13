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
    internal class Tomoe : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 3;
         
            Item.rare = ItemRarityID.Purple;
          
        }

        public override void AddRecipes()
        {
            Recipe HashiramaCell = CreateRecipe();
            HashiramaCell.AddTile(ModContent.TileType<SharinganCraftingStation>());
            HashiramaCell.Register();
        }
    }
}
