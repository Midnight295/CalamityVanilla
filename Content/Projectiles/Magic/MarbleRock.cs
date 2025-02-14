using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public class MarbleRock : ModProjectile
    {
        private ref float FrameCount => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.05f;

            FrameCount++;
            if (FrameCount > 15)
            {
                Projectile.velocity.X *= 0.97f;
                Projectile.velocity.Y += 0.2f;
            }


            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }
    }
}
