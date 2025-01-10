using CalamityVanilla.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public abstract class FrigidflashBoltProjectile : ModProjectile
    {
        private ref float HomingStrength => ref Projectile.ai[0];
        private NPC HomingTarget
        {
            get => Projectile.ai[1] == 0 ? null : Main.npc[(int)Projectile.ai[1] - 1];
            set
            {
                Projectile.ai[1] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;
            Projectile.penetrate = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            float maxRange = 800f;

            if (HomingTarget != null &&
                (
                HomingTarget.CanBeChasedBy(Projectile, false) ||
                Projectile.DistanceSQ(HomingTarget.Center) < maxRange*maxRange ||
                Collision.CanHit(Projectile.Center, 1, 1, HomingTarget.position, HomingTarget.width, HomingTarget.height
                )
            ))
            {
                HomingTarget = null;
            }

            if (HomingTarget == null)
            {
                int target = Projectile.FindTargetWithLineOfSight(maxRange);
                if (target >= 0)
                {
                    HomingTarget = Main.npc[target];
                }
            }

            if (HomingTarget == null)
                return;

            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(HomingStrength)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
            return false;
        }
    }

    public class FrigidflashColdBoltProjectile : FrigidflashBoltProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.coldDamage = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 115)
                return;
            base.AI();

            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.25f, 1.75f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center - velocity,
                    ModContent.DustType<SuperMagicIceDust>(),
                    velocity * 0.4f,
                    0,
                    default,
                    Main.rand.NextFloat(1f, 1.5f));
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.FrostHydra,
                    -Projectile.velocity.RotatedByRandom(0.4),
                    0,
                    default,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89 with
            {
                Pitch = 0.8f,
                PitchVariance = 0.2f,
                Volume = 0.6f,

                MaxInstances = 0,
            }, Projectile.Center);
            for (int k = 0; k < 8; k++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 6f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<SuperMagicIceDust>(),
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
            }
            for (int k = 0; k < 8; k++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 8f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.FrostHydra,
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 360);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn2, 360, false, false);
        }
    }

    public class FrigidflashHotBoltProjectile : FrigidflashBoltProjectile
    {
        public override void AI()
        {
            if (Projectile.timeLeft > 115)
                return;
            base.AI();

            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.25f, 1.75f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center - velocity,
                    ModContent.DustType<SuperMagicFireDust>(),
                    velocity * 0.4f,
                    0,
                    default,
                    Main.rand.NextFloat(1f, 1.5f));
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.InfernoFork,
                    -Projectile.velocity.RotatedByRandom(0.4),
                    0,
                    default,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item38 with
            {
                Pitch = 0.8f,
                PitchVariance = 0.2f,
                Volume = 0.8f,

                MaxInstances = 0,
            }, Projectile.Center);
            for (int k = 0; k < 8; k++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 6f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<SuperMagicFireDust>(),
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
            }
            for (int k = 0; k < 8; k++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 8f);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.InfernoFork,
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 360);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire3, 360, false, false);
        }
    }
}
