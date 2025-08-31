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
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Biomes.CaveHouse;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public abstract class TheSnowmanSnowball : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];
        public bool CanRoll
        {
            get => Projectile.ai[1] == 1;
            set => Projectile.ai[1] = value ? 1 : 0;
        }

        private bool ShouldSpawnDustOnKill { get; set; } = true;

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(DefaultWidth, DefaultHeight);
            Projectile.friendly = true;
            Projectile.penetrate = PenetrationAmount;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Timer++;

            if (Timer > 5)
            {
                Projectile.velocity.Y += 0.2f;

                Projectile.rotation += Projectile.velocity.X * 0.1f;
            }

            if (Projectile.shimmerWet)
            {
                Projectile.velocity.Y -= 0.5f;
            }

            //if (Projectile.velocity.Y > 0)
            //{
             //   Collision.StepDown(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
            //}

            if (CanRoll)
            {
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (CanRoll)
            {
                Projectile.scale -= 0.01f;
                Projectile.width = (int)(DefaultWidth * Projectile.scale);
                Projectile.height = (int)(DefaultHeight * Projectile.scale);

                if (Projectile.scale <= 0f)
                {
                    ShouldSpawnDustOnKill = false;
                    Projectile.Kill();
                }

                return false;
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (!ShouldSpawnDustOnKill)
            {
                return;
            }

            SoundEngine.PlaySound(SoundID.Item51 with
            {
                PitchRange = (0.1f, 0.4f),
                Volume = 0.6f,
                MaxInstances = 0,
            }, Projectile.Center);
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
                dust.noGravity = true;
                dust.velocity -= Projectile.oldVelocity * 0.25f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> asset = TextureAssets.Projectile[Type];
            Color color = Projectile.GetAlpha(lightColor);

            Main.spriteBatch.Draw(asset.Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, asset.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public abstract int DefaultWidth { get; }
        public abstract int DefaultHeight { get; }
        public abstract int PenetrationAmount { get; }
    }

    public class TheSnowmanSnowballBig : TheSnowmanSnowball
    {
        public override int DefaultWidth => 18;
        public override int DefaultHeight => 18;
        public override int PenetrationAmount => 4;
    }

    public class TheSnowmanSnowballMedium : TheSnowmanSnowball
    {
        public override int DefaultWidth => 14;
        public override int DefaultHeight => 14;
        public override int PenetrationAmount => 2;
    }

    public class TheSnowmanSnowballSmall : TheSnowmanSnowball
    {
        public override int DefaultWidth => 12;
        public override int DefaultHeight => 12;
        public override int PenetrationAmount => 1;
    }

    public class TheSnowmanSnowballTiny : TheSnowmanSnowball
    {
        public override int DefaultWidth => 8;
        public override int DefaultHeight => 8;
        public override int PenetrationAmount => 0;
    }
}
