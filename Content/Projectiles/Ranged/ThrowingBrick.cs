using CalamityVanilla.Common;
using CalamityVanilla.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class ThrowingBrick : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(false, 24);
            //Projectile.penetrate = 3;
        }
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.TrailingMode[Type] = 2;
            //ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }
        public override void OnKill(int timeLeft)
        {
            //SoundEngine.PlaySound(SoundID.Tink, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item127, Projectile.position);
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(Projectile.Hitbox), DustID.Clay, Main.rand.NextVector2Circular(2, 2));
                d.noGravity = !Main.rand.NextBool(3);
                d.velocity -= Projectile.velocity * 0.2f;
            }
        }
        public override void AI()
        {
            Projectile.rotation += 0.4f * (float)Projectile.velocity.X/16f;
            Projectile.ai[1] += 1f; // Use a timer to wait 15 ticks before applying gravity.
            if (Projectile.ai[1] >= 5f)
            {
                Projectile.ai[1] = 5f;
                Projectile.velocity.Y += 0.5f;
                Projectile.velocity.X *= 0.98f;
            }

            if (Collision.SolidCollision(Projectile.position - new Vector2(Projectile.width / 1.5f, Projectile.height / 1.5f), (int)(Projectile.width * 1.5f), (int)(Projectile.height * 1.5f)))
            {
                OnTileCollide(Projectile.oldVelocity);
            }

            // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster.
            // Past 16f this projectile will travel through blocks, so this check is useful.
            if (Projectile.velocity.Y > 16f) 
            {
                Projectile.velocity.Y = 16f;
            }
            if (Math.Abs(Projectile.velocity.X) > 16f) Projectile.velocity.X = 16f * Math.Sign(Projectile.velocity.X);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool hitTile = false;
            int rand = Main.rand.Next(1, 3);
            Point tileCoord = (Projectile.Center).ToTileCoordinates();
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (Main.tile[tileCoord.X + i, tileCoord.Y + j].TileType == ModContent.TileType<CryogenIceTile>() || Main.tile[tileCoord.X + i, tileCoord.Y + j].TileType is TileID.Glass or TileID.BreakableIce or TileID.MagicalIceBlock or TileID.Waterfall or TileID.Lavafall or TileID.Honeyfall or TileID.SandFallBlock or TileID.Confetti or TileID.ConfettiBlack or TileID.BlueStarryGlassBlock or TileID.GoldStarryGlassBlock or TileID.SnowFallBlock)
                    {
                        if (Math.Abs(i) < 2 && Math.Abs(j) < 2) // 100% chance to break inner radius
                        {
                            WorldGen.KillTile(tileCoord.X + i, tileCoord.Y + j);
                        }
                        else
                        {
                            if (Main.rand.Next(0, 10) > 3) // chance to break outer radius
                            {
                                WorldGen.KillTile(tileCoord.X + i, tileCoord.Y + j);
                            }
                        }
                        Projectile.velocity = Projectile.oldVelocity;
                        if (Projectile.ai[0] < rand && !hitTile)
                        {
                            Projectile.ai[0]++;
                            hitTile = true;
                            return false;
                        }
                    }
                }
            }
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            return true;
        }
    }
}
