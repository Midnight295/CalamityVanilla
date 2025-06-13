using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class CrystalineShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(false,16);
            Projectile.timeLeft = 20;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Opacity = Projectile.timeLeft / 20f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int length = ProjectileID.Sets.TrailCacheLength[Type];
            for (int i = 1; i < length; i++)
            {
                Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value, Projectile.oldPos[i] - Main.screenPosition + (Projectile.Size / 2), null, new Color(0.2f, 0.6f, 0.8f, 0f) * Projectile.Opacity * (1f - (i / (float)length)), Projectile.oldRot[i], TextureAssets.Extra[ExtrasID.ThePerfectGlow].Size() / 2, new Vector2(0.5f,1f) * Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            }
            Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 1f, 1f, 0f) * Projectile.Opacity * 2f, Projectile.rotation, TextureAssets.Extra[ExtrasID.ThePerfectGlow].Size() / 2, 0.75f * Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            //for (int i = 0; i < 2; i++)
            //{
            //    Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 1f, 1f, 0f) * Projectile.Opacity * 0.1f, (i * MathHelper.PiOver2), TextureAssets.Extra[ExtrasID.ThePerfectGlow].Size() / 2, new Vector2(0.4f,3f) * Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            //}
            return false;
        }
    }
}
