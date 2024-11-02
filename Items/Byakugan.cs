using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sharingan.Buffs;
using Sharingan.Items.Ingredients;
using Sharingan.Tiles.Furniture;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace Sharingan.Items
{

    internal class Byakugan : ModItem
    {
     
        public static readonly int MoveSpeedBonus = 5;
        internal int ShadowDodgeTimer = 0;
        private bool IsShadowDodgeActive = false;

       
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = 100000;
            Item.rare = ItemRarityID.Master;
            Item.lifeRegen = 10;
        }

       

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyModPlayerByakugan>().SharinganEquipped = true;
            player.GetCritChance(DamageClass.Generic) += 1f;
           
          
            player.moveSpeed += MoveSpeedBonus / 20f;
            player.jumpSpeedBoost += 2;


            

           /* player.AddBuff(BuffID.Hunter, 68);
            player.AddBuff(BuffID.WaterWalking, 68);
            player.AddBuff(BuffID.NightOwl, 68);
            player.AddBuff(BuffID.Shine, 68);
            player.AddBuff(BuffID.Dangersense, 68);
            player.AddBuff(BuffID.MagicPower, 68);
            player.AddBuff(BuffID.Spelunker, 68);*/
           

            player.GetModPlayer<PlayerImmunityCustomByakugan>().ImmunityAcc = true;

            player.AddBuff(ModContent.BuffType<ByakuganBuff>(), 1);

        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.Frozen, 160);
            target.AddBuff(BuffID.Confused, 60);
        }
        public override void AddRecipes()
        {
            Recipe Byakugan = CreateRecipe();
            Byakugan.AddIngredient(ItemID.Sapphire, 10);
            Byakugan.AddIngredient(ItemID.Gel, 100);
            Byakugan.AddIngredient(ItemID.Lens, 15);
            Byakugan.AddIngredient(ItemID.MeteoriteBar, 20);
            Byakugan.AddIngredient(ModContent.ItemType<PurifiedEye>(), 1);
            Byakugan.AddTile(ModContent.TileType<SharinganCraftingStation>());
            Byakugan.Register();
        }

       

    }



    

    public class PlayerImmunityCustomByakugan : ModPlayer
    {
        public bool ImmunityAcc;
        
      
        public override void ResetEffects()
        {
            ImmunityAcc = false;
        }

    
        public override void PostHurt(Player.HurtInfo info)
        {
           
            if (!ImmunityAcc)
            {
                return;
            }

          
            if (!info.PvP)
            {
                Player.AddImmuneTime(info.CooldownCounter, 60);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
           
            bool hasAccessoryEquipped = false;

            foreach (var item in Player.armor)
            {
                if (item.type == ModContent.ItemType<Byakugan>()) 
                {
                    hasAccessoryEquipped = true;
                    break;
                }
            }

            if (hasAccessoryEquipped)
            {
                target.AddBuff(BuffID.Confused, 60); 
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
           Player.ClearBuff(BuffID.ShadowDodge);
            
        }
    }


    public class MyModPlayerByakugan : ModPlayer
    {
       
        public bool SharinganEquipped;
       

        public override void PreUpdateBuffs()
        {
          
        }
        private void HideBuff(int buffID)
        {
            int index = Player.FindBuffIndex(buffID);
            if (index != -1)
            {
                Player.DelBuff(index); 
                Player.buffTime[index] = 2; 
            }
        }
        public override void ResetEffects()
        {
            SharinganEquipped = false; 
           
        }

        public override void PreUpdate()
        {
            base.PreUpdate();
      
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {

            foreach (var item in Player.armor)
            {
                if (item.type == ModContent.ItemType<Byakugan>())
                {
                    SharinganEquipped = true;
                    break;
                }
                
            }

            if (SharinganEquipped)
            {
               

                drawInfo.colorEyes = new Color(207, 238, 245);
            }
        }
    }


}


