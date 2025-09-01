using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles
{
    public class ParasiticBeltEffect : ModProjectile
    {
        public ref float Siner => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = ProjAIStyleID.BrainofConfusion;
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            Projectile.position.X += ((float)Math.Cos(Siner / 5f) * 2f / (Siner / 10f)) - 2f / (Siner / 5f);
            Projectile.rotation = (float)Math.Sin(Siner / 5f) * (1f - Siner/60f);
        }
    }
}