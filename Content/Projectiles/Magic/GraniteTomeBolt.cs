using CalamityVanilla.Content.Dusts;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public class GraniteTomeBolt : ModProjectile
    {
        private static VertexStrip _vertexStrip = new VertexStrip();

        private int ActualTotalTimeleft => ProjectileID.Sets.TrailCacheLength[Projectile.type] * 2;
        private int TotalTimeLeft => ProjectileID.Sets.TrailCacheLength[Projectile.type];
        private bool CanSpawnMoreBolts => Projectile.ai[0] < 3;

        public override void Load()
        {
            GameShaders.Misc["GraniteTome"] = new MiscShaderData(Main.VertexPixelShaderRef, "MagicMissile").UseProjectionMatrix(true);
            GameShaders.Misc["GraniteTome"].UseImage0("Images/Extra_195");
            GameShaders.Misc["GraniteTome"].UseImage1("Images/Extra_195");
            GameShaders.Misc["GraniteTome"].UseImage2("Images/Extra_197");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 128;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = ActualTotalTimeleft;
            Projectile.extraUpdates = 1;// 7;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.coldDamage = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > ActualTotalTimeleft - 8)
                return;
            if (Projectile.timeLeft < TotalTimeLeft)
            {
                Projectile.friendly = false;
                Projectile.velocity = Vector2.Zero;
                return;
            }
            if (Projectile.timeLeft % 16 == 0)
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(0.1);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.timeLeft % 32 == 0 && CanSpawnMoreBolts)
                {
                    Projectile.ai[0]++;
                    double rotationRadians = Main.rand.NextFloat(0.5f, 1f) * (Main.rand.NextBool() ? 1 : -1);
                    Projectile newProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(rotationRadians), Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0]);
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

        public override void PostDraw(Color lightColor)
        {

            Color StripColors(float p) => new Color(p, 1f, 1f, 1f);
            float StripWidth(float p) => 24f * (1 - p);

            MiscShaderData miscShaderData = GameShaders.Misc["GraniteTome"];
            miscShaderData.UseSaturation(-1f);
            miscShaderData.UseOpacity(2f);
            miscShaderData.Apply(null);
            _vertexStrip.PrepareStrip(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2f, Projectile.oldPos.Length);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }
    }
}
