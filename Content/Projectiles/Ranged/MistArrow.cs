using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class MistArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
            Projectile.arrow = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainCloud);
            d.velocity *= 0.1f;
            d.velocity += Projectile.velocity * 0.2f;
            d.alpha = 128;
            d.noGravity = true;

            if (Main.rand.NextBool(5))
            {
                Dust d2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
                d2.velocity *= 0.1f;
                d2.velocity += Projectile.velocity * 0.2f;
                d2.noGravity = true;
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 30)
            {
                Projectile.velocity.Y += 0.1f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for(int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainCloud);
                d.noGravity = true;
                d.velocity *= 1.5f;
                d.alpha = 128;
            }
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
                d.noGravity = true;
                d.velocity *= 1.5f;
            }
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }
}
