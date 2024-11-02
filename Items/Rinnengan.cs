using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sharingan.Buffs;
using Sharingan.Items.Ingredients;
using Sharingan.Projectiles;
using Sharingan.Tiles.Furniture;
using System;
using System.Linq;

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.RGB;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Sharingan.Items
{   

    internal class Rinnengan : ModItem, ILocalizedModType
    {

        public static readonly int MoveSpeedBonus = 5;
        internal int ShadowDodgeTimer = 0;
        private bool IsShadowDodgeActive = false;

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            var assignedKeysTP = Sharingan.TeleportKeyBind.GetAssignedKeys();
            string hotkeyTextTP = assignedKeysTP.Count > 0 ? assignedKeysTP[0].ToString() : "None";
            var assignedKeysST = Sharingan.ShinraTenseiKeyBind.GetAssignedKeys();
            string hotkeyTextST = assignedKeysST.Count > 0 ? assignedKeysST[0].ToString() : "None";
            var assignedKeysAT = Sharingan.AttractEnemyKeyBind.GetAssignedKeys();
            string hotkeyTextAT = assignedKeysAT.Count > 0 ? assignedKeysAT[0].ToString() : "None";
            var assignedKeysPE = Sharingan.PushEnemyKeyBind.GetAssignedKeys();
            string hotkeyTextPE = assignedKeysPE.Count > 0 ? assignedKeysPE[0].ToString() : "None";


            list.Add(new TooltipLine(Mod, "AwakeningLevel", "[c/FF0000:Awakening level: 4]")); 
            list.Add(new TooltipLine(Mod, "PushEnemy", $"[c/FFFF00:Press {hotkeyTextPE} to push the enemy away]"));
            list.Add(new TooltipLine(Mod, "AttractEnemy", $"[c/FFFF00:Hold {hotkeyTextAT} to attract the enemy]"));
            list.Add(new TooltipLine(Mod, "Teleport", $"[c/FFFF00:Press {hotkeyTextTP} to teleport to cursor]"));
            list.Add(new TooltipLine(Mod, "Hotkey", $"[c/FFFF00:Press {hotkeyTextST} to use Shinra Tensei]")); 
            list.Add(new TooltipLine(Mod, "ExplosionWarning", "[c/FFFF00:Very big explosion. Use it at your own risk!]"));
            list.Add(new TooltipLine(Mod, "ManaCost", "[c/00FFFF:Consumes mana]"));
            list.Add(new TooltipLine(Mod, "EyeOfSansara", "[c/FF4500:Eye of Sansara]"));
        }


        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[BuffID.WaterWalking] = true;
            Main.buffNoSave[BuffID.WaterWalking] = true;
            Main.buffNoTimeDisplay[BuffID.NebulaUpDmg2] = true;
            Main.buffNoSave[BuffID.NebulaUpDmg2] = true;
            Main.buffNoTimeDisplay[BuffID.NebulaUpLife2] = true;
            Main.buffNoSave[BuffID.NebulaUpLife2] = true;
            Main.buffNoTimeDisplay[BuffID.NebulaUpMana2] = true;
            Main.buffNoSave[BuffID.NebulaUpMana2] = true;
            Main.buffNoTimeDisplay[BuffID.Regeneration] = true;
            Main.buffNoSave[BuffID.Regeneration] = true;
            Main.buffNoTimeDisplay[BuffID.ManaRegeneration] = true;
            Main.buffNoSave[BuffID.ManaRegeneration] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = 100000;
            Item.rare = ItemRarityID.Master;
            Item.lifeRegen = 10;
            Item.crit = 20;
           // Item.defense = 40;

        }

       

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 7;
            player.noFallDmg = true;
            ShadowDodgeTimer++;
            player.GetDamage(DamageClass.Generic) += 1.5f;
          //  player.endurance = 1f - (0.1f * (1f - player.endurance));
            player.GetModPlayer<DashPlayerRinnengan>().DashAccessoryEquipped = true;
            player.moveSpeed += MoveSpeedBonus / 4f;
            player.jumpSpeedBoost += 6;

            

            player.AddBuff(ModContent.BuffType<RinnenganBuff>(), 1);
            player.GetModPlayer<PlayerImmunityCustomRinnengan>().ImmunityAcc = true;

            player.GetModPlayer<MyModPlayerRinnengan>().SharinganEquipped = true;

           

            /* if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.O))
             { 
                 player.GetModPlayer<MyModPlayerRinnengan>().ReviveAllDeadTownNPCs();
             }*/

            if (Sharingan.PushEnemyKeyBind.JustPressed && player.GetModPlayer<MyModPlayerRinnengan>().cooldown <= 0)
                {
                    player.GetModPlayer<MyModPlayerRinnengan>().KnockbackNearbyNPCs(500f);
                    player.AddBuff(ModContent.BuffType<PushDebuff>(), 300);
                    player.GetModPlayer<MyModPlayerRinnengan>().cooldown = 300;
                }
                if (Sharingan.AttractEnemyKeyBind.Current && player.GetModPlayer<MyModPlayerRinnengan>().cooldown <= 0 && player.statMana >= 10)
                {
                    player.statMana -= 1;
                    player.manaRegenDelay = 10;
                    player.GetModPlayer<MyModPlayerRinnengan>().PullNearbyNPCs(500f);
                    player.GetModPlayer<MyModPlayerSharingan>().OnDodge();
                    
                   

                if (player.GetModPlayer<MyModPlayerRinnengan>().projID == -1)
                    {
                        player.GetModPlayer<MyModPlayerRinnengan>().projID = 0;

                        Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BlackHoleProjectile>(), 50, 0, player.whoAmI);
                    }
                }
                else
                {
                    if (player.GetModPlayer<MyModPlayerRinnengan>().projID != -1)
                    {
                        player.GetModPlayer<MyModPlayerRinnengan>().RemoveBlackHole();
                        player.GetModPlayer<MyModPlayerRinnengan>().projID = -1;
                    }
                 player.GetModPlayer<MyModPlayerSharingan>().OffDodge();

                }

                if (Sharingan.ShinraTenseiKeyBind.JustPressed && player.GetModPlayer<MyModPlayerRinnengan>().cooldownTimer <= 0)
                {
                    player.GetModPlayer<MyModPlayerRinnengan>().ApplyLayeredExplosion();
                    SoundEngine.PlaySound(SoundID.Item14);
                    player.GetModPlayer<MyModPlayerRinnengan>().brightnessTimer = player.GetModPlayer<MyModPlayerRinnengan>().brightnessDuration;
                    player.AddBuff(ModContent.BuffType<ShinraTenseiDebuff>(), 18000);
                    player.GetModPlayer<MyModPlayerRinnengan>().isScreenBrightened = true;
                    player.GetModPlayer<MyModPlayerRinnengan>().cooldownTimer = 3600;
                }

                if (Sharingan.TeleportKeyBind.JustPressed && player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().tpCooldownTimer <= 0)
                {
                    player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().TeleportToCursor();
                    player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().tpCooldownTimer = 300;
                    SoundEngine.PlaySound(SoundID.Item8);
                    player.AddBuff(ModContent.BuffType<TpRinnCooldownBuff>(), 300);

                }


           
            if (player.GetModPlayer<MyModPlayerRinnengan>().cooldown > 0)
                {
                    player.GetModPlayer<MyModPlayerRinnengan>().cooldown--;
                }
            

        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {

            target.AddBuff(BuffID.Confused, 60);
        }
        public override void AddRecipes()
        {
            Recipe Rinnengan = CreateRecipe();
            Rinnengan.AddIngredient(ModContent.ItemType<MangekyoSharinganlvl2>(), 1);
            Rinnengan.AddIngredient(ModContent.ItemType<HashiramaCell>(), 10);
            Rinnengan.AddIngredient(ModContent.ItemType<BloodInjectionNeedle>(), 1);
         
            Rinnengan.AddTile(ModContent.TileType<SharinganCraftingStation>());
            Rinnengan.Register();
        }

    }

    public class DashPlayerRinnengan : ModPlayer
    {

        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public const int DashCooldown = 25; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int DashDuration = 70; // Duration of the dash afterimage effect in frames


        public const float DashVelocity = 20f;

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

                dust.color = Color.MediumPurple;
                dust.noGravity = true; 
                dust.fadeIn = 0.3f; 
                dust.scale = 3f;
                dust.alpha = 0; 
            }
        }
    }

    public class PlayerImmunityCustomRinnengan : ModPlayer
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


    public class MyModPlayerRinnengan : ModPlayer
    {
       
        public bool SharinganEquipped;
        public const int CooldownTime = 18000;
        public int cooldownTimer = 0;
        public int cooldown = 0;
        public bool isScreenBrightened = false; 
        public int brightnessDuration = 14; 
        public int brightnessTimer = 0;
        public const int LayerThickness = 1; 
        public const int Radius = 35;
        public float lightIntensity = 0f;
        public bool isLightActive = false;
        public bool increasing = true;

        public int projID = -1;

       

        public override void ResetEffects()
        {
            SharinganEquipped = false; 
            
            if (brightnessTimer > 0)
            {
                brightnessTimer--;
            }
            else
            {
                isScreenBrightened = false;
            }

            if (cooldownTimer > 0)
            {
                cooldownTimer--;
            }
        }

        public override void PostUpdate()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer--; 
            }

            if (isScreenBrightened)
            {
                int radius = 35; 
                for (int i = -radius; i <= radius; i++)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        if (Math.Sqrt(i * i + j * j) <= radius)
                        {
                            isLightActive = true; 
                        }
                    }
                }
            }

            if (isLightActive)
            {
                if (increasing)
                {
                    lightIntensity += 1f;
                    if (lightIntensity >= 20f)
                    {
                        increasing = false;
                    }
                }
                else
                {
                    lightIntensity -= 2;
                    if (lightIntensity <= 0f)
                    {
                        isLightActive = false;
                        lightIntensity = 0f;
                        increasing = true;
                    }
                }


                Lighting.AddLight(Player.position, lightIntensity, lightIntensity, lightIntensity);
            }
        }


        public void ReviveAllDeadTownNPCs()
        {
         
            for (int npcType = 0; npcType < NPCLoader.NPCCount; npcType++)
            {
                NPC townNPC = Main.npc.FirstOrDefault(n => n.type == npcType && n.townNPC);

              
                if (townNPC == null || !CanSpawnNPC(npcType) || IsNpcAlive(npcType))
                    continue;

              
               
                int spawnX = (int)Main.spawnTileX * 16;
                int spawnY = (int)Main.spawnTileY * 16;
                NPC.NewNPC(null, spawnX, spawnY, npcType);
                string npcName = Lang.GetNPCName(npcType).Value;
                Main.NewText($"{npcName} revived!", 255, 255, 0);
            }
        }

        private bool CanSpawnNPC(int npcType)
        {
          
            if (NPC.downedBoss1 && npcType == NPCID.Merchant) 
            {
                return true;
            }

            return true;
        }

        private bool IsNpcAlive(int npcType)
        {
         
            return Main.npc.Any(n => n.active && n.type == npcType);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {

            foreach (var item in Player.armor)
            {
                if (item.type == ModContent.ItemType<Rinnengan>())
                {
                    SharinganEquipped = true;
                    break;
                }
            }

            if (SharinganEquipped)
            {
             

                drawInfo.colorEyes = new Color(204, 153, 255);
            }

           
        }

        public override void PreUpdate()
        {
            base.PreUpdate();

           
        }

        public void RemoveBlackHole()
        {

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<BlackHoleProjectile>())
                {

                    proj.Kill();
                }
            }
        }

        public void KnockbackNearbyNPCs(float distance)
        {
            SoundEngine.PlaySound(SoundID.Item20, Player.position);

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    float distanceToNpc = Vector2.Distance(Player.Center, npc.Center);
                    if (distanceToNpc <= distance)
                    {
                        Vector2 direction = npc.Center - Player.Center;
                        direction.Normalize();
                        npc.velocity = direction * 40f; 
                    }
                }
            }
        }

        public void PullNearbyNPCs(float distance)
        {
          
            Vector2 playerDirection = Player.direction == 1 ? Vector2.UnitX : -Vector2.UnitX;

            //SoundEngine.PlaySound(SoundID.Item20, Player.position);

            Vector2 pullPosition = Player.Center + playerDirection * 78f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                   
                    Vector2 direction = pullPosition - npc.Center;
                    direction.Normalize(); 

                
                    npc.velocity = direction * 15f; 
                }
            }
        }
        public void ApplyLayeredExplosion()
        {
            Vector2 playerPosition = Player.Center;

            for (int layer = Radius; layer > 0; layer -= LayerThickness)
            {
             
                for (int x = -layer; x <= layer; x++)
                {
                    for (int y = -layer; y <= layer; y++)
                    {
                        if (Math.Sqrt(x * x + y * y) <= layer)
                        {
                            int tileX = (int)(playerPosition.X / 16f) + x; 
                            int tileY = (int)(playerPosition.Y / 16f) + y;

                            if (tileX >= 0 && tileX < Main.maxTilesX && tileY >= 0 && tileY < Main.maxTilesY)
                            {
                                Tile tile = Main.tile[tileX, tileY];
                                if (tile.HasTile)
                                {
                                    WorldGen.KillTile(tileX, tileY, false, false, true); 
                                    CreateExplosionParticles(new Vector2(tileX * 16, tileY * 16)); 
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateExplosionParticles(Vector2 position)
        {
            int dustType = DustID.Smoke; 
            int dustCount = 20; 

            for (int i = 0; i < dustCount; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                Dust dust = Dust.NewDustDirect(position, 0, 0, dustType, velocity.X, velocity.Y, 100, Color.Black, 1.5f);

                dust.noGravity = true; 
            }
        }
    }

}

    

