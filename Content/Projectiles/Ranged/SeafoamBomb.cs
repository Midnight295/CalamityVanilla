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
            Projectile.QuickDefaults(false, 16);
            Projectile.timeLeft = 4*60;
            //Projectile.penetrate = 3;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14 with { Pitch = 0.5f, PitchVariance = 0.6f }, Projectile.position);
            for (int i = 0; i < 60; i++)
            {
                int speed = 5;
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Water, Main.rand.Next(-speed, speed), Main.rand.Next(-speed*2, 1), 0, new Color(1f, 1f, 0f, 0f));
                //d.velocity *= 0.95f;
                d.scale = Main.rand.NextFloat(0.8f, 1.5f);
            }

            for (int i = 0; i < Main.rand.Next(3,6); i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-1f, 0f)), ModContent.ProjectileType<Seafoam>(), Projectile.damage/15, Projectile.knockBack, ai1: Main.rand.NextFloat(0, 3), ai2: Main.rand.NextFloat(0, 2));
            }
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity.X *= 0.99f;

            Projectile.rotation += (float)Projectile.velocity.X / 16f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnKill(0);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = Projectile.oldVelocity.Y * -0.6f;
            return false;
        }
    }
}
