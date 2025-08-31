using Microsoft.Xna.Framework;
using ReLogic.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class TheSnowmanPile : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public  float TotalTime => Projectile.ai[1];

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(34, 12);
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Timer++;
            Projectile.Opacity = Utils.GetLerpValue(TotalTime, TotalTime - 30, Timer, true);
            if (Timer > TotalTime)
            {
                Projectile.Kill();
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
    }
}
