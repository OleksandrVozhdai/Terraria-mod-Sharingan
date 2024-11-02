using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sharingan.Items.Placeable
{
    internal class SharinganCraftingStationPlaceable : ModItem
    {

        public override void SetDefaults()
        {
           
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.SharinganCraftingStation>());
            Item.width = 28;
            Item.height = 14; 
            Item.value = 150;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        public override void AddRecipes()
        {
            Recipe Laboratory = CreateRecipe();
            Laboratory.AddIngredient(ItemID.Wood, 20);
            Laboratory.AddIngredient(ItemID.IronBar, 10);
            Laboratory.AddIngredient(ItemID.Bottle, 1);
            Laboratory.AddIngredient(ItemID.Glass, 20);
            Laboratory.Register();
        }

       
    }
}
