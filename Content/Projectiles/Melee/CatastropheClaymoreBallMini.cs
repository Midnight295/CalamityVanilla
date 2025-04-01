using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class CatastropheClaymoreBallMini : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0.5f) * Projectile.Opacity;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.scale += MathF.Sin(Projectile.timeLeft * 0.1f) * 0.01f;
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowRod);
            d.velocity *= 0.3f;
            d.noGravity = true;
            d.velocity += Projectile.velocity * 0.4f;
            d.color = Color.Lerp(new Color(1f, 0.2f, 0f, 0.2f), new Color(1f, 1f, 1f, 0f), Main.rand.NextFloat(0f, 0.8f));
            d.scale = Projectile.scale;
            if(Projectile.timeLeft < 10)
            {
                Projectile.scale -= 0.1f;
            }
        }
    }
}
