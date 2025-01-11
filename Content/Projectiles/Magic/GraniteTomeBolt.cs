using CalamityVanilla.Common;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public class GraniteTomeBolt : ModProjectile
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        // to ensure that the trail mesh fades out smoothly, the projectile lives twice as long and after its timeLeft goes below TotalTimeLeft the mesh length starts to shrink
        private int ActualTotalTimeleft => ProjectileID.Sets.TrailCacheLength[Projectile.type] * 2;
        private int TotalTimeLeft => ProjectileID.Sets.TrailCacheLength[Projectile.type];

        private ref float SpawningAge => ref Projectile.ai[0];
        private bool CanSpawnMoreBolts => SpawningAge < 2;
        private ref float RandomAngleChange => ref Projectile.localAI[0];
        private ref float RandomAngleDirection => ref Projectile.ai[1];
        private ref float BoltSpawningTimeOffset => ref Projectile.ai[2];

        public override void Load()
        {
#pragma warning disable CS0618 // we're not in 1.4.5 yet
            GameShaders.Misc["GraniteTome"] = new MiscShaderData(Main.VertexPixelShaderRef, "MagicMissile").UseProjectionMatrix(true);
#pragma warning restore CS0618
            GameShaders.Misc["GraniteTome"].UseImage0("Images/Extra_197");
            GameShaders.Misc["GraniteTome"].UseImage1("Images/Extra_195");
            GameShaders.Misc["GraniteTome"].UseImage2("Images/Extra_197");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = -1; // caching positions is done manually for control
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 64;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = ActualTotalTimeleft;
            Projectile.extraUpdates = 15;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < TotalTimeLeft)
            {
                Projectile.friendly = false;
                return;
            }

            int randomTimeInterval = CanSpawnMoreBolts ? 5 : 15;
            if (Projectile.timeLeft % randomTimeInterval == 0)
            {
                if (RandomAngleChange == 0)
                    RandomAngleChange = 0.2f;
                RandomAngleChange *= RandomAngleDirection;
                RandomAngleDirection *= -1;
                Projectile.velocity = Projectile.velocity.RotatedBy(RandomAngleChange);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            }
            Projectile.oldPos[0] = Projectile.position;
            Projectile.oldRot[0] = Projectile.rotation;

            if (Projectile.owner == Main.myPlayer)
            {
                if (
                    (Projectile.timeLeft == 48 + BoltSpawningTimeOffset + TotalTimeLeft && Main.rand.NextBool(2, 3)) ||
                    (Projectile.timeLeft == 8 + BoltSpawningTimeOffset + TotalTimeLeft && Main.rand.NextBool(5))
                    )
                {
                    double rotationRadians = Main.rand.NextFloat(0.2f, 0.6f) * (Main.rand.NextBool() ? 1 : -1);
                    Vector2 velocity = Projectile.velocity.RotatedBy(rotationRadians);
                    Projectile newProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - velocity, velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.NextBool() ? 1 : -1, Main.rand.Next(-7, 8));
                    newProj.timeLeft = Projectile.timeLeft - 1;
                    newProj.netUpdate = true;
                }
            }

            /*
            float colorLerpValue = (96 - Projectile.timeLeft) / 96f - 0.1f;
            float alpha = ((96 - Projectile.timeLeft) / 80f) + 0.2f;
            float scale = ((Projectile.timeLeft) / 96f) + 0.2f;
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    ModContent.DustType<GraniteLightningDust>(),
                    Projectile.velocity.RotatedByRandom(0.2) * 0.2f + Main.rand.NextVector2Unit() * 0.2f,
                    0,
                    Color.Lerp(Color.White, Color.Lerp(Color.Cyan, Color.SlateBlue, colorLerpValue), colorLerpValue) * alpha,
                    Main.rand.NextFloat(2f, 3f) * scale
                );
                dust.noGravity = true;
            }
            */
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SpawningAge = 2;
            Projectile.velocity = Vector2.Zero;

            if (Projectile.timeLeft < TotalTimeLeft)
                return false;

            for (int i = 0; i < 7 * Projectile.timeLeft / (float)TotalTimeLeft; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, oldVelocity.X * 0.5f, oldVelocity.Y * 0.5f, 0, default, 0.5f);
                if (Main.rand.NextBool(2, 3))
                {
                    dust.noGravity = true;
                    dust.scale *= 1.5f;
                    dust.velocity *= 0.5f;
                }
            }

            return false;
        }

        public static void SpawnParticles(Vector2 position)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 2f);
                Dust dust = Dust.NewDustPerfect(position, DustID.Electric, velocity, 0, default, 0.5f);
                if (Main.rand.NextBool(2, 3))
                {
                    dust.noGravity = true;
                    dust.scale *= 1.5f;
                    dust.velocity *= 0.5f;
                }
            }

            PrettySparkleParticle prettySparkleParticle = CVParticleOrchestrator.RequestPrettySparkleParticle();
            float rotation = MathHelper.PiOver2;
            Vector2 scale = new Vector2(Main.rand.NextFloat() * 0.2f + 0.4f);
            Vector2 offset = Main.rand.NextVector2Circular(4f, 4f) * scale;
            prettySparkleParticle.ColorTint = new Color(0.3f, 1f, 1f, 0.3f);
            prettySparkleParticle.LocalPosition = position + offset;
            prettySparkleParticle.Rotation = rotation;
            prettySparkleParticle.Scale = scale;
            prettySparkleParticle.FadeInNormalizedTime = 0.05f;
            prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
            prettySparkleParticle.FadeInEnd = 10f;
            prettySparkleParticle.FadeOutStart = 20f;
            prettySparkleParticle.FadeOutEnd = 30f;
            prettySparkleParticle.TimeToLive = 30f;
            Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnParticles(Projectile.position);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CalamityVanilla.PacketType.SpawnGraniteTomeBoltSparks);
                packet.WriteVector2(Projectile.Center);
                packet.Send();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SpawnParticles(Projectile.position);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CalamityVanilla.PacketType.SpawnGraniteTomeBoltSparks);
                packet.Write(Main.myPlayer);
                packet.WriteVector2(Projectile.Center);
                packet.Send();
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Color StripColors(float p) => new Color(0.4f, 0.85f, 1f, 0f) * MathF.Sin(p*p*p * MathF.PI);
            float StripWidth(float p) => 32f;

            MiscShaderData miscShaderData = GameShaders.Misc["GraniteTome"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(Projectile.timeLeft / (float)TotalTimeLeft);
            miscShaderData.Apply(null);
            _vertexStrip.PrepareStrip(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2f, (Projectile.timeLeft > TotalTimeLeft) ? Projectile.oldPos.Length : Projectile.timeLeft);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }
    }
}
