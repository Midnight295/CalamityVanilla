using CalamityVanilla.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class TheSnowman : BaseFlailProjectile
    {
        private static Asset<Texture2D> _hat;

        public ref float HatPositionY => ref Projectile.ai[2];
        public ref float HatVelocityY => ref Projectile.localAI[2];

        public override void Load()
        {
            _hat = ModContent.Request<Texture2D>(Texture + "_Hat");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;

            for (int i = 0; i < 5; i++)
            {
                int goreType = Mod.Find<ModGore>($"TheSnowmanChain{i}").Type;

                ChildSafety.SafeGore[goreType] = true;
                GoreID.Sets.DisappearSpeedAlpha[goreType] = 25;
            }
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(26, 26);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.netImportant = true;
        }

        public override int MaxTimeLaunched => 13;
        public override float LaunchSpeed => 21;
        public override float MaxManualRetractionSpeed => 22;
        public override float MaxForcedRetractionSpeed => 25;
        public override int SpinningNPCHitCooldown => 12;


        public override void OnDropDuringLaunch()
        {
            OnDrop();
        }

        public override void OnDropDuringManualRetract()
        {
            OnDrop();
        }

        private void OnDrop()
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<TheSnowmanDropped>(), Projectile.damage, Projectile.knockBack, Projectile.whoAmI);
                projectile.localNPCImmunity = Projectile.localNPCImmunity;
                projectile.netUpdate = true;
            }

            ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;
            GenerateChainGores();
            Projectile.Kill();
        }

        private void GenerateChainGores()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
            playerArmPosition -= Vector2.UnitY * player.gfxOffY;

            float chainHeightAdjustment = ChainHeightAdjustment;

            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = 14 + chainHeightAdjustment;
            if (chainSegmentLength < 0)
            {
                chainSegmentLength = 10;
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;

            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            while (chainLengthRemainingToDraw > 0f)
            {
                int frameNumber = Math.Clamp(chainCount - 1, 0, 4);
                int goreType = Mod.Find<ModGore>($"TheSnowmanChain{frameNumber}").Type;

                Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), chainDrawPosition, Vector2.Zero, goreType);
                gore.velocity.X *= 0.2f;
                gore.rotation = chainRotation + Main.rand.NextFloat(-0.2f, 0.2f);
                gore.timeLeft = 10 + chainCount;

                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }
        }

        public override void PostAI()
        {
            HatVelocityY += 0.8f;
            HatPositionY += HatVelocityY;
            if (HatPositionY > Projectile.Top.Y)
            {
                HatPositionY = Projectile.Top.Y;
                HatVelocityY = 0f;
            }
        }

        public override Rectangle? InitialChainSourceRectangle(Asset<Texture2D> texture) => texture.Frame(1, 5, 0, 0);
        public override int ChainHeightAdjustment => -2;
        public override void PreDrawChain(int chainCount, ref Asset<Texture2D> texture, ref Rectangle? sourceRectangle, ref Color lighColor)
        {
            int frameNumber = Math.Clamp(chainCount - 1, 0, 4);
            sourceRectangle = texture.Frame(1, 5, 0, frameNumber);
        }

        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(_hat.Value, Projectile.Top - Main.screenPosition, null, Projectile.GetAlpha(lightColor), 0f, new Vector2(_hat.Width() * 0.5f, _hat.Height() - 2), 1f, SpriteEffects.None, 0);
        }
    }
}
