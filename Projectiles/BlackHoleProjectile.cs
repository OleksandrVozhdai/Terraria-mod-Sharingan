using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sharingan.Items;
using Sharingan.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

//Use this to test ProjecTile

namespace Sharingan.Projectiles
{
    internal class BlackHoleProjectile : ModProjectile
    {
        public override void SetStaticDefaults() 
        {
            Main.projFrames[Projectile.type] = 4;
          //  Main.background = true;
         

        }

        public override void SetDefaults()
        { 
          Projectile.scale = 0.5f;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.position = player.Center - new Vector2(Projectile.width / 2 - 50, Projectile.height / 2 + 10);

           
            if (player.direction == 1)
            {
                Projectile.spriteDirection = 1;  
              
            }
            else
            {
                Projectile.spriteDirection = -1; 
                Projectile.position = player.Center - new Vector2(Projectile.width / 2 + 100, Projectile.height / 2 + 10);

            }

          
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
            }

          
            Projectile.tileCollide = false;
           // Projectile.timeLeft = 100;

      
        }

        

        private void AIGeneral()
        { 
        
        }

        private void AIUpdateAnimation()
        {
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            int frameSpeed = 5;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

    }

    
}
