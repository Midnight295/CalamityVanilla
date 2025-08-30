using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class CursedDaggerExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            //Projectile.arrow = true;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            //Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 14;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 2 == 0)
            {
                Projectile.frame++;
            }
            Lighting.AddLight(Projectile.Center, new Vector3(0.9f, 1f, 0f) * Projectile.timeLeft/14f); // R G B values from 0 to 1f.
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0.5f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 4*60);
        }
    }
}