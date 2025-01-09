using CalamityVanilla.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public class FrostBoltProjectile : ModProjectile
    {
        public ref float VelocityRotationAngle => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.WindPhysicsImmunity[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterBolt);
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 5;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.90f, 0.95f, 1f) * (Projectile.timeLeft / 320));

            Projectile.velocity = Projectile.velocity.RotatedBy(VelocityRotationAngle);
            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(0.5f, 1.5f), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center - velocity,
                    ModContent.DustType<MagicIceDust>(),
                    velocity * 0.2f,
                    0,
                    default,
                    Main.rand.NextFloat(1f, 1.5f));
                dust.noGravity = true;
            }
            if (Main.rand.Next(4) == 0)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.DungeonSpirit,
                    -Projectile.velocity.RotatedByRandom(0.4),
                    0,
                    default,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 60);
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

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item100 with
            {
                Pitch = 1f,
                PitchVariance = 0.1f,
                MaxInstances = 0,
            }, Projectile.Center);
            for (int k = 0; k < 12; k++)
            {
                Vector2 velocity = new Vector2(5, 0).RotatedByRandom(MathHelper.TwoPi);
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.DungeonSpirit,
                    velocity,
                    0,
                    Color.White,
                    Main.rand.NextFloat(1.25f, 1.75f)
                );
                dust.noGravity = true;
            }
        }
    }
}
