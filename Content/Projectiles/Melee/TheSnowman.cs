using CalamityVanilla.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class TheSnowman : BaseFlailProjectile
    {
        private static Asset<Texture2D> _hat;

        public ref float HatRotation => ref Projectile.localAI[2];

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

            string[] snowmallBallGores = ["TheSnowmanCarrot", "TheSnowmanHat", "TheSnowmanBigButton", "TheSnowmanSmallButton"];
            foreach (string goreName in snowmallBallGores)
            {
                int goreType = Mod.Find<ModGore>(goreName).Type;

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

        public override bool ShouldFreelyRotate => false;


        public override void PostAI()
        {
            float intendedRotation = -Projectile.velocity.X / 20;
            HatRotation = MathHelper.Lerp(HatRotation, intendedRotation, 0.25f);
            HatRotation = Math.Clamp(HatRotation, -1f, 1f);

            float rotationAmount = (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f;
            Projectile.rotation += rotationAmount * Math.Sign(Projectile.velocity.X);
            if (CurrentAIState == AIState.Spinning)
            {
                Projectile.rotation += MathHelper.TwoPi / 15f * Main.player[Projectile.owner].direction;
            }
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CurrentAIState == AIState.Dropping)
            {
                GenerateChainGores();

                int snowballAmount = Main.rand.Next(6, 15);
                if (Main.myPlayer == Projectile.owner)
                {
                    for (int i = 0; i < snowballAmount; i++)
                    {
                        int projectileType = i switch
                        {
                            < 2 => ModContent.ProjectileType<TheSnowmanSnowballBig>(),
                            < 5 => ModContent.ProjectileType<TheSnowmanSnowballMedium>(),
                            < 9 => ModContent.ProjectileType<TheSnowmanSnowballSmall>(),
                            _ => ModContent.ProjectileType<TheSnowmanSnowballTiny>(),
                        };

                        Vector2 snowballVelocity = -oldVelocity.SafeNormalize(-Vector2.UnitY).RotatedByRandom(1f) * Main.rand.NextFloat(4f, 8f);

                        Projectile.NewProjectileDirect(
                            Projectile.GetSource_FromThis(),
                            Projectile.Center,
                            snowballVelocity,
                            projectileType,
                            (int)(Projectile.damage * 0.5f),
                            Projectile.knockBack * 0.5f,
                            Projectile.owner,
                            ai1: Main.rand.Next(2)
                        );
                    }
                }

                Vector2[] pileOffsets = [new Vector2(-16, 0), new Vector2(16, 0), new Vector2(0, 0)];
                Vector2 pileAnchor = Projectile.BottomLeft + Projectile.velocity - new Vector2(2, 0);
                foreach (Vector2 offset in pileOffsets)
                {
                    const int collisionWidthConstraint = 8;
                    if (Collision.SolidCollision(pileAnchor + offset + new Vector2(collisionWidthConstraint, 0), 34 - collisionWidthConstraint * 2, 8))
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile.NewProjectileDirect(
                                Projectile.GetSource_FromThis(),
                                pileAnchor + offset + new Vector2(15, -4),
                                Vector2.Zero,
                                ModContent.ProjectileType<TheSnowmanPile>(),
                                0,
                                0,
                                Projectile.owner,
                                ai1: Main.rand.Next(180, 240)
                            );
                        }

                        for (int i = 0; i < 16; i++)
                        {
                            Dust dust = Dust.NewDustDirect(pileAnchor + offset + new Vector2(0, -8), 34, 12, DustID.Snow, Scale: 1.5f);
                            dust.noGravity = true;
                        }
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow, Scale: 2f);
                    dust.noGravity = true;
                }

                int hatGoreType = Mod.Find<ModGore>("TheSnowmanHat").Type;
                Vector2 hatDirection = (HatRotation - MathHelper.PiOver2).ToRotationVector2();
                Gore hatGore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center + hatDirection * 15, Vector2.Zero, hatGoreType);
                hatGore.velocity *= 1.2f;
                hatGore.timeLeft = 90;

                string[] snowmanBallGores = [
                    "TheSnowmanBigButton", "TheSnowmanBigButton",
                    "TheSnowmanCarrot",
                    "TheSnowmanSmallButton", "TheSnowmanSmallButton", "TheSnowmanSmallButton", "TheSnowmanSmallButton", "TheSnowmanSmallButton", "TheSnowmanSmallButton",
                ];
                foreach (string goreName in snowmanBallGores)
                {
                    int goreType = Mod.Find<ModGore>(goreName).Type;
                    Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, goreType);
                    gore.timeLeft = 60 + Main.rand.Next(-10, 11);
                }

                SoundEngine.PlaySound(SoundID.NPCDeath15, Projectile.Center);

                return true;
            }

            return base.OnTileCollide(oldVelocity);
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
            Vector2 hatDirection = (HatRotation - MathHelper.PiOver2).ToRotationVector2();

            Main.spriteBatch.Draw(_hat.Value, Projectile.Center + hatDirection * 12 - Main.screenPosition, null, Projectile.GetAlpha(lightColor), HatRotation, new Vector2(_hat.Width() * 0.5f, _hat.Height() - 2), 1f, SpriteEffects.None, 0);
        }
    }
}
