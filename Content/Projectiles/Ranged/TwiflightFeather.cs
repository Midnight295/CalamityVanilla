using CalamityVanilla.Common;
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
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class TwiflightFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 320;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2);
                d.velocity = Main.rand.NextVector2Circular(6, 6);
                d.noGravity = true;
                d.color = Color.Lerp(new Color(0.6f, 1f, 1f), Color.MediumVioletRed, Main.rand.NextFloat()) with { A = 0 };
                d.fadeIn = Main.rand.NextFloat(0, 1.5f);
            }

            PrettySparkleParticle sparkle = CVParticleOrchestrator.RequestPrettySparkleParticle();
            sparkle.LocalPosition = Projectile.Center;
            sparkle.Scale = new Vector2(6f,1.3f);
            sparkle.Rotation = MathHelper.PiOver2 + Main.rand.NextFloat(-0.2f, 0.2f);
            sparkle.DrawVerticalAxis = true;
            sparkle.ColorTint = new Color(1f, 0.5f, 0f, 0f);
            sparkle.FadeInEnd = 5;
            sparkle.FadeOutStart = 5;
            sparkle.FadeOutEnd = 20;
            Main.ParticleSystem_World_OverPlayers.Add(sparkle);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(0, Main.rand.NextFloat(-5, 5)).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                Dust d2 = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(Main.rand.NextFloat(-7, 7), 0).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                d2.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d2.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.Y != Projectile.velocity.Y)
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
            if (oldVelocity.X != Projectile.velocity.X)
                Projectile.velocity.X = -Projectile.oldVelocity.X;

            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            Projectile.penetrate--;
            if(Projectile.penetrate == 0)
            {
                Projectile.Kill();
            }
            else
            {
                PrettySparkleParticle sparkle = CVParticleOrchestrator.RequestPrettySparkleParticle();
                sparkle.LocalPosition = Projectile.Center;
                sparkle.Scale = new Vector2(3f, 1f);
                sparkle.Rotation = Projectile.rotation;
                sparkle.ColorTint = new Color(1f, 0.5f, 0f, 0f);
                sparkle.FadeInEnd = 5;
                sparkle.FadeOutStart = 5;
                sparkle.FadeOutEnd = 20;
                sparkle.DrawVerticalAxis = false;
                Main.ParticleSystem_World_OverPlayers.Add(sparkle);
                for (int i = 0; i < 10; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(Main.rand.NextFloat(-5, 5),0).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PrettySparkleParticle sparkle = CVParticleOrchestrator.RequestPrettySparkleParticle();
            sparkle.LocalPosition = Projectile.Center;
            sparkle.Scale = new Vector2(3f, 1f);
            sparkle.Rotation = Projectile.rotation + MathHelper.PiOver2;
            sparkle.ColorTint = new Color(1f, 0.5f, 0f, 0f);
            sparkle.FadeInEnd = 5;
            sparkle.FadeOutStart = 5;
            sparkle.FadeOutEnd = 20;
            sparkle.DrawVerticalAxis = false;
            Main.ParticleSystem_World_OverPlayers.Add(sparkle);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GemTopaz, new Vector2(Main.rand.NextFloat(-5, 5), 0).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
            }
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame == 4)
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool())
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2);
                d.velocity *= 0.3f;
                d.noGravity = true;
                d.color = Color.Lerp(new Color(0.6f, 1f, 1f), Color.MediumVioletRed, Main.rand.NextFloat()) with { A = 0 };
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
            {
                float multiply = (1 - (i / 5f));
                Main.EntitySpriteDraw(tex.Value, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, new Rectangle(0, tex.Height() / 4 * Projectile.frame, tex.Width(), tex.Height() / 4), new Color(0.6f * multiply, 0f, 1f, 0f) * multiply * 0.5f, Projectile.rotation, tex.Size() / new Vector2(2, 8), 1f + (multiply * 0.2f), SpriteEffects.None);
            }

            Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, tex.Height() / 4 * Projectile.frame, tex.Width(), tex.Height() / 4), new Color(1f, 1f, 1f, 1f), Projectile.rotation, tex.Size() / new Vector2(2, 8), 1f, SpriteEffects.None);
            return false;
        }
    }
}
