using CalamityVanilla.Content.Dusts;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
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
        private bool CanSpawnMoreBolts => Projectile.ai[0] < 2;

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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 128;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = ActualTotalTimeleft;
            Projectile.extraUpdates = 31;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < TotalTimeLeft)
            {
                Projectile.friendly = false;
                Projectile.velocity = Vector2.Zero;
                Projectile.extraUpdates = 7;
                return;
            }

            if (Projectile.timeLeft % 7 == 0)
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(0.2);
            }
            else if (Projectile.timeLeft % 14 == 0 && CanSpawnMoreBolts)
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(0.8);
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
                if (CanSpawnMoreBolts && (
                    (Projectile.timeLeft == 80 + TotalTimeLeft && Main.rand.NextBool(2, 3)) ||
                    (Projectile.timeLeft == 48 + TotalTimeLeft && Main.rand.NextBool(5))
                    ))
                {
                    Projectile.ai[0]++;
                    double rotationRadians = Main.rand.NextFloat(0.3f, 0.6f) * (Main.rand.NextBool() ? 1 : -1);
                    Vector2 velocity = Projectile.velocity.RotatedBy(rotationRadians);
                    Projectile newProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - velocity, velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
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
            Projectile.ai[0] = 2;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Color StripColors(float p) => new Color(0.4f, 0.85f, 1f, 0f) * (1 - p*p*p);
            float StripWidth(float p) => 32f * MathF.Cbrt(p);

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
