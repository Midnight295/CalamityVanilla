using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class Seafoam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        
        public ref float Size => ref Projectile.ai[1];
        public ref float FrameNum => ref Projectile.ai[2];

        public int rand = Main.rand.Next(80, 120);

        public override void SetDefaults()
        {
            //Projectile.arrow = true;
            Projectile.width = 24;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            //Projectile.DamageType = DamageClass.Ranged;
            //Projectile.timeLeft = (int)(rand * 4f);
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.95f;

            Projectile.ai[0]++;
            if (Projectile.ai[0] % 8 == 0)
            {
                FrameNum++;
            }

            if (FrameNum > 2)
            {
                FrameNum = 0;
            }

            if (Projectile.ai[0] % rand == 0)
            {
                Size++;
            }

            if (Size > 3)
            {
                Size = 3;
            }

            if (Projectile.ai[0] > rand * 4f)
            {
                Projectile.timeLeft = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Vector2 drawPos = (Projectile.Center - Main.screenPosition) + Projectile.Size + new Vector2(3, 8);
            float sinVal = ((float)Math.Sin(Projectile.ai[0] / 15f) / 5f + 0.75f);
            Color color = Projectile.GetAlpha(lightColor) * (1f / Math.Clamp(Projectile.ai[0]/2f - rand, 1, rand*4)) * sinVal;
            Main.EntitySpriteDraw(texture, drawPos, texture.Frame(3, 4, (int)FrameNum, (int)Size), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = (float)Math.Round(Projectile.ai[0] / rand * 4f, 0, MidpointRounding.AwayFromZero) * rand;
        }
    }
}