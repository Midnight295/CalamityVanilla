﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged
{
    public class FleshLauncher : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(ProjectileID.PurificationPowder, ItemID.Grenade, singleShotTime: 50, shotVelocity: 4f, hasAutoReuse: true);
            Item.width = 62;
            Item.height = 30;
            Item.damage = 10;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.buyPrice(gold: 7);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Grenade)
            {
                type = ModContent.ProjectileType<Fleshnade>();
            }
            else if (type == ProjectileID.StickyGrenade)
            {
                type = ModContent.ProjectileType<FleshnadeSticky>();
            }
            else if (type == ProjectileID.BouncyGrenade)
            {
                type = ModContent.ProjectileType<FleshnadeBouncy>();
            }
            else if (type == ProjectileID.Beenade)
            {
                type = ModContent.ProjectileType<FleshnadeBee>();
            }
            //position += new Vector2(0, MathHelper.SmoothStep(-10, 10, player.itemAnimation / (float)player.itemAnimationMax)).RotatedBy(velocity.ToRotation());
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
    }

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

    public class FleshnadeSticky : Fleshnade
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

    public class FleshnadeBouncy : Fleshnade
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

    public class FleshnadeBee : Fleshnade
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
