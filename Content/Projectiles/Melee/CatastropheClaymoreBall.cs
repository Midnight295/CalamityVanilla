using CalamityVanilla.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class CatastropheClaymoreBall : ModProjectile
    {
        const byte FRIGHT = 0;
        const byte PLIGHT = 1;
        const byte LIGHT = 2;
        const byte NIGHT = 3;
        const byte MIGHT = 4;
        const byte FLIGHT = 5;
        const byte WHITE = 6;
        const byte SIGHT = 7;
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(false, 16);
            Projectile.timeLeft = 90;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            switch (Projectile.ai[0])
            {
                case MIGHT:
                    Projectile.velocity *= 1.05f;
                    Projectile.velocity = Projectile.velocity.LengthClamp(32);
                    break;
                case PLIGHT:
                case WHITE:
                    Projectile.velocity *= 1.05f;
                    Projectile.velocity = Projectile.velocity.LengthClamp(20);
                    break;
                case FLIGHT:
                    Projectile.velocity.Y -= 0.1f;
                    Projectile.velocity = Projectile.velocity.LengthClamp(32);
                    break;
                case SIGHT:
                    int target = Projectile.FindTargetWithLineOfSight(800);
                    if (target == -1)
                        break;
                    NPC n = Main.npc[target];
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(n.Center) * Projectile.velocity.Length() * 12, 0.01f);
                    Projectile.velocity = Projectile.velocity.LengthClamp(12);
                    break;
            }

            Projectile.scale += MathF.Sin(Projectile.timeLeft * 0.1f) * 0.01f;

            if (Projectile.ai[1] < 5)
                return;

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowRod);
            d.velocity *= 0.3f;
            d.noGravity = true;
            d.velocity += Projectile.velocity * 0.4f;
            d.color = Color.Lerp(GetDustColor(), new Color(1f, 1f, 1f, 0f), Main.rand.NextFloat(0f, 0.8f));
            d.scale = Projectile.scale;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            switch (Projectile.ai[0])
            {
                case FRIGHT:
                    modifiers.ScalingBonusDamage += -0.2f;
                    break;
                //case LIGHT:
                //    modifiers.Knockback *= Projectile.extraUpdates * 0.5f;
                //    break;
                case MIGHT:
                    modifiers.Knockback *= Projectile.velocity.Length() / 8f;
                    break;
                case FLIGHT:
                    modifiers.ScalingBonusDamage += 0.1f;
                    break;
                case SIGHT:
                    modifiers.ScalingBonusDamage += -0.1f;
                    break;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == LIGHT)
            {
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
                for (int i = 0; i < 15; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowRod, Main.rand.NextVector2Circular(4, 2).RotatedBy(Projectile.rotation));
                    d.color = Color.Lerp(GetDustColor(), new Color(1f, 1f, 1f, 0f), Main.rand.NextFloat(0f, 0.8f));
                    d.noGravity = true;
                }

                if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }

                if (Projectile.velocity.X != Projectile.oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                Projectile.extraUpdates++;
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (Projectile.ai[0])
            {
                case PLIGHT:
                    target.AddBuff(BuffID.OnFire3, 60 * 3);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(8, 8));
                        d.noGravity = true;
                        d.scale = 2f;
                    }
                    break;
                case NIGHT:
                    bool debuff = Main.rand.NextBool();
                    target.AddBuff(debuff? BuffID.Ichor : BuffID.CursedInferno, 60 * 3);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, debuff ? DustID.IchorTorch : DustID.CursedTorch, Main.rand.NextVector2Circular(4, 4));
                        d.noGravity = !Main.rand.NextBool(3);
                        d.scale = Main.rand.NextFloat(0.5f,1.5f);
                    }
                    Projectile.penetrate = -1;
                    break;
                case WHITE:
                    target.AddBuff(BuffID.Frostburn2, 60 * 3);
                    for (int i = 0; i < 15; i++)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.IceTorch, Main.rand.NextVector2Circular(8, 8));
                        d.noGravity = true;
                        d.scale = 1f;
                        Dust d2 = Dust.NewDustPerfect(Projectile.Center, DustID.Snow, Main.rand.NextVector2Circular(8, 8));
                        d2.noGravity = true;
                        d2.scale = 1f;
                    }
                    break;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == FRIGHT && Main.myPlayer == Projectile.owner)
            {
                float rotation = Main.rand.NextFloat(-0.1f, 0.1f);
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(4,8), 0).RotatedBy(i * MathHelper.PiOver2 + rotation), ModContent.ProjectileType<CatastropheClaymoreBallMini>(), Projectile.damage / 6, Projectile.knockBack / 4, Projectile.owner);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                PrettySparkleParticle sparkle = CVParticleOrchestrator.RequestPrettySparkleParticle();
                sparkle.ColorTint = GetDustColor();
                sparkle.LocalPosition = Projectile.Center;
                sparkle.Scale = Projectile.ai[0] == MIGHT? new Vector2(Main.rand.NextFloat(4f, 5f), Main.rand.NextFloat(1.5f, 2f)) : new Vector2(Main.rand.NextFloat(2f, 3.5f), Main.rand.NextFloat(0.6f, 1f));
                sparkle.RotationVelocity = Main.rand.NextFloat(0.2f, -0.2f);
                sparkle.RotationAcceleration = -sparkle.RotationVelocity / 120f;
                sparkle.FadeInNormalizedTime = 0.05f;
                sparkle.FadeOutNormalizedTime = 0.95f;
                sparkle.FadeInEnd = sparkle.FadeOutStart = Main.rand.NextFloat(5,10);
                sparkle.FadeOutEnd = sparkle.TimeToLive = Main.rand.NextFloat(30, 60);
                sparkle.AdditiveAmount = 0.5f;
                Main.ParticleSystem_World_OverPlayers.Add(sparkle);
            }
            if (Projectile.ai[0] == MIGHT)
            {
                for (int i = 0; i < 15; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowRod, Main.rand.NextVector2Circular(8, 8));
                    d.color = Color.Lerp(GetDustColor(), new Color(1f, 1f, 1f, 0f), Main.rand.NextFloat(0f, 0.8f));
                    d.noGravity = true;
                    d.scale += Main.rand.NextFloat();
                }
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowRod, Main.rand.NextVector2Circular(4, 4));
                    d.color = Color.Lerp(GetDustColor(), new Color(1f, 1f, 1f, 0f), Main.rand.NextFloat(0f, 0.8f));
                    d.noGravity = true;
                }
                SoundEngine.PlaySound(SoundID.Item73 /*with { MaxInstances = 10, PitchVariance = 0.3f}*/, Projectile.position);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];

            Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, tex.Frame(8, Main.projFrames[Type], (int)Projectile.ai[0], Projectile.frame), new Color(1f,1f,1f,0.5f) * Projectile.Opacity, Projectile.rotation, new Vector2(9, 27),Projectile.scale * (Projectile.ai[0] == MIGHT ? 1.3f : 1f),SpriteEffects.None);
            return false;
        }
        private Color GetDustColor()
        {
            Color color = Color.White;

            switch (Projectile.ai[0])
            {
                case FRIGHT:
                    color = new Color(1f, 0.2f, 0f);
                    break;
                case PLIGHT:
                    color = new Color(1f, 0f, 0.1f);
                    break;
                case LIGHT:
                    color = new Color(1f, 0.2f, 1f);
                    break;
                case NIGHT:
                    color = new Color(0.5f, 0f, 1f);
                    break;
                case MIGHT:
                    color = new Color(0f, 0.3f, 1f);
                    break;
                case FLIGHT:
                    color = new Color(0f, 0.9f, 1f);
                    break;
                case WHITE:
                    color = new Color(0.5f, 0.7f, 1f); // WDYM IT'S NOT WHITE
                    break;
                case SIGHT:
                    color = new Color(0f, 1f, 0.1f);
                    break;
            }
            color.A = 64;
            return color;
        }
    }
}
