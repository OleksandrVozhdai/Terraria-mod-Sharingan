using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Sharingan
{
    public class Sharingan : Mod
    {
        public static ModKeybind ShinraTenseiKeyBind, PushEnemyKeyBind, AttractEnemyKeyBind, TeleportKeyBind,
                                 ShadowDodgeKeyBind, AmaterasuKeyBind, CrowsKeyBindSpawn, CrowsKeyBindDeSpawn,
                                 GenjutsuSharingan;

        public override void Load()
        {
            ShinraTenseiKeyBind = KeybindLoader.RegisterKeybind(this, "Rinnengan - Shinra Tensei", Microsoft.Xna.Framework.Input.Keys.P);
            PushEnemyKeyBind = KeybindLoader.RegisterKeybind(this, "Rinnengan - Push enemy", Microsoft.Xna.Framework.Input.Keys.F);
            AttractEnemyKeyBind = KeybindLoader.RegisterKeybind(this, "Rinnengan - Attract enemy", Microsoft.Xna.Framework.Input.Keys.G);
            TeleportKeyBind = KeybindLoader.RegisterKeybind(this, "Rinn/Shar - Teleport to cursor", Microsoft.Xna.Framework.Input.Keys.Q);
            ShadowDodgeKeyBind = KeybindLoader.RegisterKeybind(this, "Sharingan - Dodge", Microsoft.Xna.Framework.Input.Keys.F);
            AmaterasuKeyBind = KeybindLoader.RegisterKeybind(this, "Sharingan - Amaterasu", Microsoft.Xna.Framework.Input.Keys.G);
            CrowsKeyBindSpawn = KeybindLoader.RegisterKeybind(this, "Sharingan - Spawn ravens", Microsoft.Xna.Framework.Input.Keys.K);
            CrowsKeyBindDeSpawn = KeybindLoader.RegisterKeybind(this, "Sharingan - Despawn ravens", Microsoft.Xna.Framework.Input.Keys.Y);
            GenjutsuSharingan = KeybindLoader.RegisterKeybind(this, "Sharingan - Genjutsu Sharingan", Microsoft.Xna.Framework.Input.Keys.L);
           // TextureAssets.Moon = ModContent.Request<Texture2D>("Sharingan/Images/MoonRinn").Value;    
        }

        public override void Unload()
        {
      
        }



    }


}