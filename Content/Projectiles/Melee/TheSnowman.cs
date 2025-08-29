using CalamityVanilla.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class TheSnowman : BaseFlailProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
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

        public override Rectangle? InitialChainSourceRectangle(Asset<Texture2D> texture) => texture.Frame(1, 4);
        public override int ChainHeightAdjustment => -6;
        public override void PreDrawChain(int chainCount, ref Asset<Texture2D> texture, ref Rectangle? sourceRectangle, ref Color lighColor)
        {
            int frameNumber = 4 - Math.Clamp(chainCount - 1, 0, 4);
            sourceRectangle = texture.Frame(1, 5, 0, frameNumber);
        }
    }
}
