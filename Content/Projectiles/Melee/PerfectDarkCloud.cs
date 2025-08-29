using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class PerfectDarkCloud : ModProjectile
    {
        public float TotalTime => Projectile.ai[0];
        public ref float Timer => ref Projectile.localAI[0];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.frame = Main.rand.Next(3);
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override void AI()
        {
            Timer++;

            Projectile.Opacity = Utils.GetLerpValue(0, 30, Timer, true) * Utils.GetLerpValue(TotalTime, TotalTime - 30, Timer, true);
            if (Timer > TotalTime)
            {
                Projectile.Kill();
                return;
            }

            Projectile.velocity *= 0.98f;
            Projectile.rotation += Projectile.velocity.X * 0.02f;
        }
    }
}
