using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class Crystaline : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
        }
        public override void AI()
        {
            base.AI();
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
    }
}
