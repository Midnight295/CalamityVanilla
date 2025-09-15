using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Magic
{
    // ExampleStaff is a typical staff. Staffs and other shooting weapons are very similar, this example serves mainly to show what makes staffs unique from other items.
    // Staff sprites, by convention, are angled to point up and to the right. "Item.staff[Type] = true;" is essential for correctly drawing staffs.
    // Staffs use mana and shoot a specific projectile instead of using ammo. Item.DefaultToStaff takes care of that.
    public class PyrobatStaff : ModItem
    {
        public const int HoldoutDistance = 20;

        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true; // This makes the useStyle animate as a staff instead of as a gun.
        }

        public override void SetDefaults()
        {
            // DefaultToStaff handles setting various Item values that magic staff weapons use.
            // Hover over DefaultToStaff in Visual Studio to read the documentation!
            //Item.DefaultToStaff(ModContent.ProjectileType<Pyrobat>(), 16, 50, 25);
            Item.mana = 25;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.UseSound = SoundID.Item95;
            Item.SetWeaponValues(85, 8);
            Item.SetShopValues(ItemRarityColor.LightRed4, 60000);
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.shoot = ModContent.ProjectileType<PyrobatStaffHeldProjectile>();
            Item.useStyle = ItemUseStyleID.Shoot;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Since this item will attempt to shoot an ammo item, we need to set it back to the actual held projectile here.
            type = ModContent.ProjectileType<PyrobatStaffHeldProjectile>();

            // The velocity value provided is not correct, so we need to calculate a new velocity since velocity for held projectiles is actually the holdout offset.
            velocity = velocity.SafeNormalize(Vector2.Zero) * HoldoutDistance;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer);
            return false;
        }

        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);
        }
    }

    public class PyrobatStaffHeldProjectile : ModProjectile
    {
        public ref float ShootTimer => ref Projectile.ai[0];
        public int ShootCount = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 60;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            //Projectile.hide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;

            // Adjust the drawing to change how it appears when held
            //DrawOffsetX = -17;
        }

        // Held projectiles should set this to false otherwise they will deal contact damage.
        public override bool? CanDamage() => false;

        public override void AI()
        {
            Projectile.frame = 0; // set frame to full

            // See ExampleDrillProjectile.cs for comments on code common to held projectiles. The comments in this file will focus on the unique aspects of this projectile.
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            // HoldTimer counts how long the weapon has been used. It helps control how fast the weapon animates and shoots arrows.
            ShootTimer += 1f;
            int useTime = Projectile.timeLeft;

            if (ShootTimer < 40)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 pos = player.Center + new Vector2(35, (player.direction == 1) ? -5 : 5).RotatedBy(Projectile.rotation).RotatedBy(-MathHelper.PiOver4).RotatedBy((player.direction == -1) ? -MathHelper.PiOver2 : 0);
                    Dust d = Dust.NewDustDirect(pos, 5, 5, DustID.Smoke);
                    d.velocity.X *= 0.2f;
                    d.velocity.Y *= 0.5f;
                    d.velocity.Y -= 0.4f;
                    d.alpha = 128;
                } 
            }

            else if (ShootTimer == 40)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 pos = player.position + new Vector2(30, (player.direction == 1) ? 10 : -10).RotatedBy(Projectile.rotation).RotatedBy(-MathHelper.PiOver4).RotatedBy((player.direction == -1) ? -MathHelper.PiOver2 : 0);
                    Dust d = Dust.NewDustDirect(pos, 20, 20, DustID.Torch);
                    d.velocity *= Main.rand.NextFloat(0.3f, 2.5f);
                    d.scale = Main.rand.NextFloat(0.5f, 1.6f);
                }

                if (ShootTimer < 20)
                {
                    Vector2 pos = player.position + new Vector2(30, (player.direction == 1) ? 10 : -10).RotatedBy(Projectile.rotation).RotatedBy(-MathHelper.PiOver4).RotatedBy((player.direction == -1) ? -MathHelper.PiOver2 : 0);
                    Dust d = Dust.NewDustDirect(pos, 30, 30, DustID.Smoke);
                    d.velocity *= Main.rand.NextFloat(1f, 2.5f);
                    d.alpha = 128;
                }

                // SPAWN BATS
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

                if (Main.rand.NextBool(5))
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + (20 * player.direction), Projectile.position.Y - 10), new Vector2(5f * player.direction, 0f), ModContent.ProjectileType<Pyrobat2>(), Projectile.damage, Projectile.knockBack);
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + (20 * player.direction), Projectile.position.Y - 10), new Vector2(5f * player.direction, 0f), ModContent.ProjectileType<Pyrobat>(), Projectile.damage, Projectile.knockBack);
                }
            }
            else if (ShootTimer >= 40) // set frame to empty
            {
                Projectile.frame = 1;
            }
            else if (ShootTimer >= useTime)
            {
                ShootTimer = 0f;
            }

            if (ShootTimer < 2) // rotate towards mouse with clamping
            {
                DrawOriginOffsetX = (player.direction == 1) ? -10 : 10;
                DrawOriginOffsetY = -10;
                Projectile.rotation = MathHelper.Clamp((Main.MouseWorld - player.Center).ToRotation(), -20, 0) + ((player.direction == 1) ? MathHelper.PiOver4 : 3 * MathHelper.PiOver4) + (((Main.MouseWorld - player.Center).ToRotation() > 0 && player.direction == -1) ? MathHelper.Pi : 0);
                Projectile.direction = player.direction;
                Projectile.spriteDirection = Projectile.direction;
            }

            //player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            Projectile.Center = playerCenter + new Vector2(20 * player.direction, 5);
            //Main.NewText(MathHelper.ToDegrees(Projectile.rotation));

        }
    }

    public class Pyrobat : ModProjectile
    {
        public float startY;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60*5;
            DrawOffsetX = -25;
            DrawOriginOffsetY = -20;

        }

        public override void OnSpawn(IEntitySource source)
        {
            startY = Projectile.position.Y;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, 20, 20, DustID.Torch);
                d.velocity *= Main.rand.NextFloat(0.3f, 1.5f);
                d.scale = Main.rand.NextFloat(0.5f, 1.2f);
                d.noGravity = Main.rand.NextBool(2);
            }
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.75f, 0.25f);

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            Projectile.ai[0]++;
            Projectile.position.Y = startY + (float)Math.Sin(Projectile.ai[0] / 5f) * 15f;

            if (Projectile.ai[1]-- <= 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<PyrobatFlame>(), Projectile.damage / 2, Projectile.knockBack);
                Projectile.ai[1] = Main.rand.Next(15, 40);
            }

            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, (int)(Projectile.width * 2f), (int)(Projectile.height * 2f), DustID.Torch);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1f, 1.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = oldVelocity.X * -1f;
            return false;
        }
    }

    public class Pyrobat2 : Pyrobat { } // functionally identical to Pyrobat, except for the sprite

    public class PyrobatFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1; // or 14
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            //AIType = ProjectileID.GreekFire1;
            Projectile.timeLeft = 60 * 6;
            Projectile.penetrate = Main.rand.Next(2,3);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 60 * 2);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = oldVelocity.Y * -0.24f;
            Projectile.velocity.X += Main.rand.NextFloat(-0.75f, 0.75f);
            return false;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.velocity.X *= 0.2f;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
            }

            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, (int)(Projectile.width * 1.5f), (int)(Projectile.height * 1.5f), DustID.Torch);
                d.scale = Main.rand.NextFloat(1f, 1.5f);
                d.noGravity = true;
                d.velocity.X *= 0.2f;
                d.velocity.Y -= 1f;
            }
        }
    }
}