﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Sharingan.Tiles.Furniture;

namespace Sharingan.Items.Ingredients
{
    internal class BloodInjectionNeedle : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.sellPrice(0,0,1,0);
            Item.rare = ItemRarityID.Purple;
        }

        public override void AddRecipes()
        {
            Recipe Injection = CreateRecipe();
            Injection.AddTile(ModContent.TileType<SharinganCraftingStation>());
            Injection.AddIngredient(ItemID.Glass, 10);
            Injection.AddIngredient(ItemID.IronBar, 1);
            Injection.Register();
        }
    }
}
