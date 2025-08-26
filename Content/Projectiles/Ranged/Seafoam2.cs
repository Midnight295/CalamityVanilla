using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class Seafoam2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
            ProjectileID.Sets.NoLiquidDistortion[Type] = true;
        }
        public ref float Size => ref Projectile.ai[1];
        public ref float FrameNum => ref Projectile.ai[2];

        public int rand = Main.rand.Next(80, 120);
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.velocity.Y += -0.1f;
            if (Projectile.velocity.Y < -2f * rand / 60f)
                Projectile.velocity.Y = -2f * rand / 60f;
            Projectile.velocity.X *= 0.98f;
            if(!Projectile.wet)
            {
                Projectile.Kill();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - Projectile.velocity, Projectile.velocity * Vector2.UnitX, ModContent.ProjectileType<Seafoam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1], Projectile.ai[2]);
            }
                Projectile.ai[0]++;
            if (Projectile.ai[0] % 8 == 0)
            {
                FrameNum++;
            }

            if (FrameNum > 2)
            {
                FrameNum = 0;
            }

            if (Projectile.ai[0] % rand == 0)
            {
                Size++;
            }

            if (Size > 3)
            {
                Size = 3;
            }

            if (Projectile.ai[0] > rand * 4f)
            {
                Projectile.timeLeft = 0;
            }
            if (Projectile.ai[0] > rand * 3.7f)
            {
                Projectile.Opacity *= 0.9f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 6, texture.Height / 8);
            Vector2 drawPos = (Projectile.Center - Main.screenPosition);
            float sinVal = ((float)Math.Sin(Projectile.ai[0] / 15f) / 5f + 0.75f);
            Color color = Projectile.GetAlpha(lightColor) * sinVal * Projectile.Opacity;
            Main.EntitySpriteDraw(texture, drawPos, texture.Frame(3, 4, (int)FrameNum, (int)Size), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -Projectile.oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = (float)Math.Round(Projectile.ai[0] / rand * 4f, 0, MidpointRounding.AwayFromZero) * rand;
        }
    }
}