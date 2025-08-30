using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class TheSnowmanDropped : ModProjectile
    {
        public ref float Timer => ref Projectile.ai[0];

        public override string Texture => "CalamityVanilla/Content/Projectiles/Melee/TheSnowman";

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

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent { Entity: Projectile parentProjectile })
            {
                for (int i = 0; i < parentProjectile.localNPCImmunity.Length; i++)
                {
                    //Projectile.localNPCImmunity[i] = parentProjectile.localNPCImmunity[i];
                }
            }
        }

        public override void AI()
        {
            Timer++;

            // The dropped variant should have identical physics to dropped flails
            // So, the physics code isn't run when it first spawns to account for the fact that the flail wouldn't be dropped on that frame
            if (Timer > 1)
            {
                if (!Projectile.shimmerWet)
                {
                    Projectile.velocity.Y += 0.8f;
                }
                Projectile.velocity.X *= 0.95f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Point scanAreaStart = Projectile.TopLeft.ToTileCoordinates();
            Point scanAreaEnd = Projectile.BottomRight.ToTileCoordinates();

            bool wasFastEnoughOnX = Math.Abs(oldVelocity.X) > 4f;
            bool wasFastEnoughOnY = Math.Abs(oldVelocity.Y) > 4f;
            if (wasFastEnoughOnX || wasFastEnoughOnY)
            {

            }
            Dust.QuickBox(Projectile.TopLeft + Projectile.velocity, Projectile.BottomRight + Projectile.velocity, 1, Color.White, null);

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            
        }
    }
}
