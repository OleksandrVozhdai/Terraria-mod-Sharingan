using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Sharingan.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Sharingan;
using Sharingan.Projectiles;
using Sharingan.Tiles.Furniture;
using System.Collections.Generic;

namespace Sharingan.Items
{


    public class MangekyoSharinganlvl2 : ModItem, ILocalizedModType
    {

        public static readonly int MoveSpeedBonus = 5;
        internal int ShadowDodgeTimer = 0;
        public bool IsShadowDodgeActive = false;

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            var assignedKeyGenjutsuSharingan = Sharingan.GenjutsuSharingan.GetAssignedKeys();
            string hotKeyGenjutsuSharingan = assignedKeyGenjutsuSharingan.Count > 0 ? assignedKeyGenjutsuSharingan[0].ToString() : "None";
            var assignedKeysTP = Sharingan.TeleportKeyBind.GetAssignedKeys();
            string hotkeyTextTP = assignedKeysTP.Count > 0 ? assignedKeysTP[0].ToString() : "None";
            var assignedKeysAM = Sharingan.AmaterasuKeyBind.GetAssignedKeys();
            string hotkeyTextAM = assignedKeysAM.Count > 0 ? assignedKeysAM[0].ToString() : "None";
            var assignedKeysDodge = Sharingan.ShadowDodgeKeyBind.GetAssignedKeys();
            string hotkeyTextDodge = assignedKeysDodge.Count > 0 ? assignedKeysDodge[0].ToString() : "None";
            var assignedKeysSR = Sharingan.CrowsKeyBindSpawn.GetAssignedKeys();
            string hotkeyTextSR = assignedKeysSR.Count > 0 ? assignedKeysSR[0].ToString() : "None";
            var assignedKeysDR = Sharingan.CrowsKeyBindDeSpawn.GetAssignedKeys();
            string hotkeyTextDR = assignedKeysDR.Count > 0 ? assignedKeysDR[0].ToString() : "None";

            list.Add(new TooltipLine(Mod, "AwakeningLevel", "[c/FF0000:Awakening level: 3]"));
            list.Add(new TooltipLine(Mod, "DodgeAttack", $"[c/FFFF00:Hold {hotkeyTextDodge} to dodge attacks with your Sharingan]"));
            list.Add(new TooltipLine(Mod, "ActivateAmaterasu", $"[c/FFFF00:Press {hotkeyTextAM} to activate Amaterasu]"));
            list.Add(new TooltipLine(Mod, "SummonRavens", $"[c/FFFF00:Press {hotkeyTextSR} to summon ravens, {hotkeyTextDR} for despawn (Takes minion slot)]"));
            list.Add(new TooltipLine(Mod, "IzanagiDojutsu", "[c/FFFF00:Izanagi Dojutsu (Passive)]"));
            list.Add(new TooltipLine(Mod, "TeleportToCursor", $"[c/FFFF00:Press {hotkeyTextTP} to teleport to cursor]"));
            list.Add(new TooltipLine(Mod, "Genjutsu Sharingan", $"[c/FFFF00:Press {hotKeyGenjutsuSharingan} to activate genjutsu]"));
            list.Add(new TooltipLine(Mod, "ManaCost", "[c/00FFFF:Consumes mana]"));
            list.Add(new TooltipLine(Mod, "TrueSharingan", "[c/FF4500:True Sharingan]"));
        }


        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[BuffID.WaterWalking] = true;
            Main.buffNoSave[BuffID.WaterWalking] = true;
            Main.buffNoTimeDisplay[BuffID.Hunter] = true;
            Main.buffNoSave[BuffID.Hunter] = true;
            Main.buffNoTimeDisplay[BuffID.Rage] = true;
            Main.buffNoSave[BuffID.Rage] = true;
            Main.buffNoTimeDisplay[BuffID.Blackout] = true;
            Main.buffNoSave[BuffID.Blackout] = true;
        }
       

        public override void SetDefaults()
        {
            //  Main.buffNoTimeDisplay[Type] = true;

            Item.accessory = true;
            Item.lifeRegen = 10;
        //    Item.crit = 20;
           // Item.damage = 50;
            Item.value = 1000000;
            Item.rare = ItemRarityID.Master;
            Item.lifeRegen = 10;

          //  Item.shoot = ProjectileID.Raven;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            // ShadowDodgeTimer++;
          

            if (player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().IzanagiCoolDownTimer == 0)
            {
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().SharinganEquipped = true;
            }
            else if (player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().IzanagiCoolDownTimer == 17999)
            {
                player.AddBuff(ModContent.BuffType<IzanagiDeBuff>(), player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().IzanagiCoolDownTimer);
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().SharinganEquipped = false;
            }

            player.noFallDmg = true;
            player.maxMinions += 6;
            player.GetDamage(DamageClass.Generic) += 1f;
          //  player.endurance = 1f - (0.1f * (1f - player.endurance));
            player.GetModPlayer<DashPlayerMangekyo>().DashAccessoryEquipped = true;
            player.GetCritChance(DamageClass.Generic) += 1f;
            player.moveSpeed += MoveSpeedBonus / 5f;
            player.jumpSpeedBoost += 3;

            if (Sharingan.AmaterasuKeyBind.JustPressed && player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().cooldownTimer <= 0)
            {
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().isLightActive = true;
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().ApplyOnFireBuffToNearbyEnemies();
                player.AddBuff(BuffID.Blackout, 3600);
                player.AddBuff(ModContent.BuffType<AmaterasuDebuff>(), 3600);

                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().cooldownTimer = 3600;
                SoundEngine.PlaySound(SoundID.Item20, player.position);
            }

            if (Sharingan.GenjutsuSharingan.JustPressed && player.GetModPlayer<MyModPlayerSharingan>().cooldown <= 0)
            {
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().isLightActive = true;
                player.GetModPlayer<MyModPlayerSharingan>().FreezeEnemiesGenjutsu();
                player.AddBuff(ModContent.BuffType<GenjutsuCoolDown>(), 3600);
            }

            if (Sharingan.ShadowDodgeKeyBind.Current && player.statMana >= 10)
            {

                player.statMana -= 1;
                player.manaRegenDelay = 10;
                player.GetModPlayer<MyModPlayerSharingan>().OnDodge();

            }
            else
            {
                player.GetModPlayer<MyModPlayerSharingan>().OffDodge();
            }

            if (Sharingan.TeleportKeyBind.JustPressed && player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().tpCooldownTimer <= 0)
            {
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().TeleportToCursor();
                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().tpCooldownTimer = 300;
                SoundEngine.PlaySound(SoundID.Item8);
                player.AddBuff(ModContent.BuffType<TpCooldownBuff>(), 300);
            }

            if (Sharingan.CrowsKeyBindSpawn.JustPressed)
            {

                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().SummonRavens();
            }
            if (Sharingan.CrowsKeyBindDeSpawn.JustPressed)
            {

                player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().RemoveRavens();
            }

            //player.AddBuff(BuffID.Ravens, 2);
            player.AddBuff(ModContent.BuffType<MangekyoSharinganlvl2Buff>(), 1);


            player.GetModPlayer<MyModPlayerMangekyoSharinganlvl2>().hasAccessoryEquipped = true;
            player.GetModPlayer<PlayerImmunityCustomMangekyo>().ImmunityAcc = true;

        }

       

      

        public override void AddRecipes()
        {
            Recipe MangekyoSharingan = CreateRecipe();
            MangekyoSharingan.AddIngredient(ModContent.ItemType<MangekyoSharingan>(), 1);
            MangekyoSharingan.AddIngredient(ItemID.ChlorophyteBar, 10);
            MangekyoSharingan.AddIngredient(ItemID.FragmentSolar, 5);
            MangekyoSharingan.AddIngredient(ItemID.FragmentStardust, 5);
            MangekyoSharingan.AddIngredient(ItemID.FragmentNebula, 5);
            MangekyoSharingan.AddIngredient(ItemID.FragmentVortex, 5);

            MangekyoSharingan.AddTile(ModContent.TileType<SharinganCraftingStation>());
            MangekyoSharingan.Register();
        }


        public override void OnCreated(ItemCreationContext context)
        {
                  
            foreach (NPC npc in Main.npc)
            {
                if (npc.type == NPCID.Guide && npc.active)
                {
                    npc.StrikeInstantKill(); 
                }
            }
        }

    }


    public class DashPlayerMangekyolvl2 : ModPlayer
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
            
            DashAccessoryEquipped = false;
            
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

                dust.color = Color.Red;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale = 3f;
                dust.alpha = 0;
            }
        }
    }
    public class PlayerImmunityCustomMangekyolvl2 : ModPlayer
    {
        public bool ImmunityAcc;
        private int shadowDodgeCooldown;
        public bool HasRaven;

        public override void ResetEffects()
        {
            ImmunityAcc = false;
            HasRaven = false;

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

            Player.ClearBuff(BuffID.ShadowDodge);
            shadowDodgeCooldown = 300;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            bool hasAccessoryEquipped = false;

            foreach (var item in Player.armor)
            {
                if (item.type == ModContent.ItemType<MangekyoSharingan>())
                {
                    hasAccessoryEquipped = true;
                    break;
                }
            }

            if (hasAccessoryEquipped)
            {

                target.AddBuff(BuffID.Confused, 60);
                target.AddBuff(BuffID.Slow, 60);
                target.AddBuff(BuffID.OnFire, 12000000);
            }

        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {

        }

        public override void PreUpdate()
        {
           
        }

    }
    public class MyModPlayerMangekyoSharinganlvl2 : ModPlayer
    {
        public bool SharinganEquipped;
        public bool hasAccessoryEquipped;
        public const int CooldownTime = 3600; //18000 - for izanagi
        public int IzanagiCoolDownTimer = 0;
        public int cooldownTimer = 0;
        public float lightIntensity = 0f;
        public bool isLightActive = false;
        public bool increasing = true;
        public bool IzanagiActive = false;
        public int tpCooldownTimer = 0;

        public override void OnRespawn()
        {
            if (SharinganEquipped == false)
            {
                Player.AddBuff(ModContent.BuffType<IzanagiDeBuff>(), IzanagiCoolDownTimer);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (SharinganEquipped)
            {
               
                Player.statLife = Player.statLifeMax2;
                Player.immune = true; 
                Player.immuneTime = 60; 
                IzanagiActive = true;
                IzanagiCoolDownTimer = 18000;
            
                Player.dead = false;
                Player.respawnTimer = 0;
            }
        }

        public override void ResetEffects()
        {
            SharinganEquipped = false;
        }

        public override void PostUpdate()
        {
            if (tpCooldownTimer > 0)
            { 
                tpCooldownTimer--;
            }
            if (IzanagiCoolDownTimer > 0)
            {
                IzanagiCoolDownTimer--;
            }
            if (cooldownTimer > 0)
            {
                cooldownTimer--;
            }

            if (isLightActive)
            {
                if (increasing)
                {
                    lightIntensity += 2f;
                    if (lightIntensity >= 10f)
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


                Lighting.AddLight(Player.position, lightIntensity, 0f, 0f);
            }
        }

        public void TeleportToCursor()
        {
            Vector2 oldPosition = Player.position;
            Vector2 cursorPosition = Main.MouseWorld;

       
            Player.Teleport(cursorPosition, 1, 0);
            Player.velocity = Vector2.Zero;
            SpawnRedSmoke(oldPosition);
            Vector2 velocity = new Vector2(0, -5f);
           
        }

        private void SpawnRedSmoke(Vector2 position)
        {
          
            int dustAmount = 30;

            for (int i = 0; i < dustAmount; i++)
            {
              
                Vector2 dustPosition = position + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));

              
                int dustIndex = Dust.NewDust(dustPosition, 10, 10, DustID.Smoke, 0f, 0f, 100, Color.Red, 2f);

               
                Main.dust[dustIndex].velocity *= 1.5f;
                Main.dust[dustIndex].scale = 2f;
                Main.dust[dustIndex].noGravity = true; 
            }
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (SharinganEquipped)
            {

                drawInfo.colorEyes = new Color(255, 0, 0);
            }
        }

        public void ApplyOnFireBuffToNearbyEnemies()
        {
            Vector2 playerPosition = Player.Center;
            float radius = 1000f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly)
                {
                    if (Vector2.Distance(playerPosition, npc.Center) <= radius)
                    {

                        npc.AddBuff(ModContent.BuffType<AmaterasuBuff>(), 3000000);


                        CreateBlackFireParticles(npc.Center);
                    }
                }
            }
        }

        public void CreateBlackFireParticles(Vector2 position)
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

        public override void PreUpdate()
        {
            base.PreUpdate();
        }

        public void SummonRavens()
        {
           
            if (Player.ownedProjectileCounts[ProjectileID.Raven] < 5)
            {
                for (int i = Player.ownedProjectileCounts[ProjectileID.Raven]; i < 5; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileID.Raven, 50, 0, Player.whoAmI);
                }
            }
        }

        private void SummonSusanoo()
        {
          
                for (int i = 0; i < 1; i++)
                {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<SusanooProjectile>(), 50, 0, Player.whoAmI);
            }
            
        }

        public void RemoveRavens()
        {
          
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.owner == Player.whoAmI && proj.type == ProjectileID.Raven)
                {

                    proj.Kill(); 
                }
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);
        }
        

      }
}
