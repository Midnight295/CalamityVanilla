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
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Ranged
{
    public class Fleshnade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60 * 3;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
            d.velocity *= 0.1f;
            d.alpha = 128;
            Projectile.rotation += Projectile.velocity.X * 0.07f;
            
            if (Projectile.ai[0] > 10)
            {
                Projectile.velocity.Y += 0.3f;
                Projectile.oldVelocity.Y += 0.3f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.45f;
            }

            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.6f;
            }
            Projectile.velocity.X *= 0.95f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.5f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.customData = 1;
                d.velocity *= 6f;
                d.scale = 2f;
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                d.velocity *= 2f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                d.velocity *= 3f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = false;
            }
            Projectile.Explode(16 * 5);
        }
    }

    public class StickyFleshnade : Fleshnade
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.5f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.customData = 1;
                d.velocity *= 6f;
                d.scale = 2f;
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                d.velocity *= 2f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                d.velocity *= 3f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = false;
            }
            Projectile.Explode(16 * 5);
        }
    }

    public class BouncyFleshnade : Fleshnade
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }

            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.6f;
            }
            Projectile.velocity.X *= 0.95f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.5f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.customData = 1;
                d.velocity *= 6f;
                d.scale = 2f;
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                d.velocity *= 2f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                d.velocity *= 3f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = false;
            }
            Projectile.Explode(16 * 5);
        }
    }

    public class Honeycombnade : Fleshnade
    {
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != Projectile.oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.45f;
            }

            if (Projectile.velocity.X != Projectile.oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.6f;
            }
            Projectile.velocity.X *= 0.95f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int rand = Main.rand.Next(15, 25);
                for (int i = 0; i < rand; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)), ProjectileID.Bee, Projectile.damage, Projectile.knockBack);
                }
            }

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.5f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath1 with { Volume = 0.5f }, Projectile.Center);
            for (int i = 0; i < 35; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.customData = 1;
                d.velocity *= 6f;
                d.scale = 2f;
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                d.velocity *= 2f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = !Main.rand.NextBool(3);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
                d.velocity *= 3f;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.noGravity = false;
            }
            Projectile.Explode(16 * 5);
        }
    }
}
