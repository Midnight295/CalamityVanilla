using CalamityVanilla.Common;
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
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class SeafoamBomb : ModProjectile
    {
        public override string Texture => "CalamityVanilla/Content/Items/Weapons/Ranged/SeafoamBomb";
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(false, 20);
            Projectile.timeLeft = 4*60;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14 with { Pitch = 0.5f, PitchVariance = 0.6f }, Projectile.position);
            for (int i = 0; i < 60; i++)
            {
                int speed = 5;
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Water, Main.rand.Next(-speed, speed), Main.rand.Next(-speed*2, 1), 0, new Color(1f, 1f, 0f, 0f));
                d.scale = Main.rand.NextFloat(0.8f, 1.5f);
            }

            for (int i = 0; i < Main.rand.Next(3,6); i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(5,2), ModContent.ProjectileType<Seafoam2>(), Projectile.damage/15, Projectile.knockBack, ai1: Main.rand.NextFloat(0, 3), ai2: Main.rand.NextFloat(0, 2));
            }
        }
        public override void AI()
        {

            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.99f;

            Projectile.rotation += (float)Projectile.velocity.X / 16f;
            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -10).RotatedBy(Projectile.rotation), DustID.Water, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-5, -2)), 0, new Color(1f, 1f, 0f, 0f));
                d.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + new Vector2(0,4), null, lightColor, Projectile.rotation, new Vector2(9, 17), Projectile.scale, SpriteEffects.None);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if(Projectile.velocity.Y != oldVelocity.Y)
            Projectile.velocity.Y = Projectile.oldVelocity.Y * -0.6f;
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = Projectile.oldVelocity.X * -0.6f;
            return false;
        }
    }
}
