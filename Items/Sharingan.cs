using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sharingan.Buffs;
using Sharingan.Items.Ingredients;
using Sharingan.Projectiles;
using Sharingan.Tiles.Furniture;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MonoMod.Cil;


namespace Sharingan.Items
{

    internal class SharinganItem : ModItem
    {

        private const int StopEnemiesCooldownTime = 3600; 
        private int stopEnemiesCooldown = 0;
        private static Texture2D customMoonTexture;

      

        public static readonly int MoveSpeedBonus = 5;
        internal int ShadowDodgeTimer = 0;
        private bool IsShadowDodgeActive = false;

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            var assignedKeysDodge = Sharingan.ShadowDodgeKeyBind.GetAssignedKeys();
            var assignedKeyGenjutsuSharingan = Sharingan.GenjutsuSharingan.GetAssignedKeys();
            string hotKeyGenjutsuSharingan = assignedKeyGenjutsuSharingan.Count > 0 ? assignedKeyGenjutsuSharingan[0].ToString() : "None";
            string hotkeyTextDodge = assignedKeysDodge.Count > 0 ? assignedKeysDodge[0].ToString() : "None";

            list.Add(new TooltipLine(Mod, "AwakeningLevel", "[c/FF0000:Awakening level: 1]"));
            list.Add(new TooltipLine(Mod, "DodgeAttack", $"[c/FFFF00:Hold {hotkeyTextDodge} to dodge attacks with your Sharingan]"));
            list.Add(new TooltipLine(Mod, "Genjutsu Sharingan", $"[c/FFFF00:Press {hotKeyGenjutsuSharingan} to activate genjutsu]"));

            list.Add(new TooltipLine(Mod, "ManaCost", "[c/00FFFF:Consumes mana]"));
            list.Add(new TooltipLine(Mod, "TrueSharingan", "[c/FF4500:Strength of the Uchiha clan]"));
        }

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[BuffID.WaterWalking] = true;
            Main.buffNoSave[BuffID.WaterWalking] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = 100000;
            Item.rare = ItemRarityID.Master;
            Item.lifeRegen = 10;
            Item.crit = 20;

           
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (stopEnemiesCooldown > 0)
            {
                stopEnemiesCooldown--; 
            }

            ShadowDodgeTimer++;
            player.maxMinions += 3;
            player.GetModPlayer<MyModPlayerSharingan>().SharinganEquipped = true;
            player.GetDamage(DamageClass.Generic) += 0.5f; 
            //player.endurance = 1f - (0.1f * (1f - player.endurance));  
            player.GetModPlayer<DashPlayer>().DashAccessoryEquipped = true;
            player.moveSpeed += MoveSpeedBonus / 10f;
            player.jumpSpeedBoost += 3;

            if (Sharingan.ShadowDodgeKeyBind.Current && player.GetModPlayer<MyModPlayerSharingan>().cooldown <= 0 && player.statMana >= 10)
            {
                player.statMana -= 1;
                player.manaRegenDelay = 10;
                player.AddBuff(BuffID.ShadowDodge, 600);
                player.GetModPlayer<MyModPlayerSharingan>().OnDodge();
            }
            else 
            {
                player.GetModPlayer<MyModPlayerSharingan>().OffDodge();
            }

             if (Sharingan.GenjutsuSharingan.JustPressed && player.GetModPlayer<MyModPlayerSharingan>().cooldown <= 0)
             {
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().isLightActive = true;
                player.GetModPlayer<MyModPlayerSharingan>().FreezeEnemiesGenjutsu();
                player.AddBuff(ModContent.BuffType<GenjutsuCoolDown>(), 3600);
             }


            player.AddBuff(ModContent.BuffType<SharinganBuff>(), 1);
            player.GetModPlayer<PlayerImmunityCustom>().ImmunityAcc = true;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {

            target.AddBuff(BuffID.Confused, 60);
        }
        public override void AddRecipes()
        {
            Recipe Sharingan = CreateRecipe();
            Sharingan.AddIngredient(ItemID.HellstoneBar, 25);
            Sharingan.AddIngredient(ItemID.Lens, 20);
            Sharingan.AddIngredient(ItemID.LifeCrystal, 3);
            Sharingan.AddIngredient(ItemID.Ruby, 10);
            Sharingan.AddIngredient(ModContent.ItemType<PurifiedEye>(), 1);
            Sharingan.AddTile(ModContent.TileType<SharinganCraftingStation>());
            Sharingan.Register();
        }

       

    }



    public class DashPlayer : ModPlayer
    {
       

        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public const int DashCooldown = 25; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int DashDuration = 70; // Duration of the dash afterimage effect in frames

    
        public const float DashVelocity = 15f;



        public int DashDir = -1;

     
        public bool DashAccessoryEquipped;
        public int DashDelay = 0; // frames remaining till we can dash again
        public int DashTimer = 0; // frames remaining in the dash

        public override void ResetEffects()
        {
            // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
            DashAccessoryEquipped = false;

            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
            {
                DashDir = DashDown;
            }
            else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
            {
                DashDir = DashUp;
            }
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
        }

        // This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
        // If they double tapped this frame, they'll move fast this frame
        public override void PreUpdateMovement()
        {
            // if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                    case DashUp when Player.velocity.Y > -DashVelocity:
                    case DashDown when Player.velocity.Y < DashVelocity:
                        {
                            // Y-velocity is set here
                            // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                            // This adjustment is roughly 1.3x the intended dash velocity
                            float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                            newVelocity.Y = dashDirection * DashVelocity;
                            break;
                        }
                    case DashLeft when Player.velocity.X > -DashVelocity:
                    case DashRight when Player.velocity.X < DashVelocity:
                        {
                            // X-velocity is set here
                            float dashDirection = DashDir == DashRight ? 1 : -1;
                            newVelocity.X = dashDirection * DashVelocity;
                            break;
                        }
                    default:
                        return; // not moving fast enough, so don't start our dash
                }

                // start our dash
                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;


                // Here you'd be able to set an effect that happens when the dash first activates
                CreateDashParticles();
                // Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashTimer > 0)
            { // dash is active
              // This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
              // Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
              // Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;

                // count down frames remaining
                DashTimer--;
            }
        }

        private bool CanUseDash()
        {
            return DashAccessoryEquipped
                && Player.dashType == DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                && !Player.setSolar // player isn't wearing solar armor
                && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
        }

        private void CreateDashParticles()
        {
            // Adjust the parameters to suit your needs
            int dustType = DustID.Smoke; // Type of dust; you can choose different types
            int dustCount = 20; // Number of dust particles

            for (int i = 0; i < dustCount; i++)
            {
                Vector2 position = Player.position + new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, 10));
                Vector2 velocity = Player.velocity * 0.5f; // Modify the velocity to control the movement of the dust

                Dust dust = Dust.NewDustDirect(position, Player.width, Player.height, dustType, velocity.X, velocity.Y, 100, default, 1.5f);

                dust.color = Color.DarkRed;
                dust.noGravity = true; 
                dust.fadeIn = 0.1f; 
                dust.scale = 2f;
                dust.alpha = 0;
            }
        }
    }

    public class PlayerImmunityCustom : ModPlayer
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
                if (item.type == ModContent.ItemType<SharinganItem>()) 
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

       
    }

    public class MyModPlayerSharingan : ModPlayer
    {
       
        public bool SharinganEquipped;
        public const int CooldownTime = 3600;
        public int cooldownTimer = 0;
        public bool Dodge = false;
       
        public int cooldown = 0;
        public override void ResetEffects()
        {
            SharinganEquipped = false; 
        }

        public override void PostUpdate()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer--; 
            }
            if (cooldown > 0)
            {
                cooldown--;
            }
        }


        public void FreezeEnemiesGenjutsu()
        {
            if (cooldown <= 0) 
            {
                Vector2 playerPosition = Player.Center;
                float radius = 1000f; 

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(playerPosition, npc.Center) <= radius)
                    {
                        
                        npc.AddBuff(ModContent.BuffType<GenjutsuSharingan>(), 360);
                        npc.localAI[0] = 360;
                    }
                }

                cooldown = CooldownTime; 
            }
        }


        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {

            foreach (var item in Player.armor)
            {
                if (item.type == ModContent.ItemType<SharinganItem>())
                {
                    SharinganEquipped = true;
                    break;
                }
            }

            if (SharinganEquipped)
            {
                // Change the player's eye color to red
               
                drawInfo.colorEyes = new Color(255, 0, 0);
            }
        }

        public override void PreUpdate()
        {
            base.PreUpdate();

           

            if (Dodge == true)
            {
                Player.AddBuff(BuffID.ShadowDodge, 1);
            }

        }

        public void OnDodge()
        { 
            Dodge = true;
        }

        public void OffDodge()
        {
            Dodge = false;
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);
           
        }

    }




}


