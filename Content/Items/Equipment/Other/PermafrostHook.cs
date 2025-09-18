using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Equipment.Other
{
    public class PermafrostHook : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(26, 20);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.shoot = ModContent.ProjectileType<PermafrostHookProjectile>();
            Item.shootSpeed = 18f;
        }
    }

    public class PermafrostHookProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.SingleGrappleHook[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
        }

        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner == Main.myPlayer && projectile.type == Projectile.type)
                {
                    hooksOut++;
                }
            }

            return hooksOut <= 1;
        }

        public override void UseGrapple(Player player, ref int type)
        {
            
        }

        public override float GrappleRange()
        {
            return 450f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 20f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 14;
        }

        public override bool PreDrawExtras()
        {
            Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>("CalamityVanilla/Content/Items/Equipment/Other/PermafrostHookChain");

            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 projectileCenter = Projectile.Center;
            Vector2 directionToPlayer = Main.player[Projectile.owner].MountedCenter - projectileCenter;

            float rotationToPlayer = directionToPlayer.ToRotation() - MathHelper.PiOver2;
            float distanceToPlayer = directionToPlayer.Length();

            while (distanceToPlayer > 25f && !float.IsNaN(distanceToPlayer))
            {
                directionToPlayer /= distanceToPlayer; // Turns directionToPlayer into a unit vector to...
                directionToPlayer *= chainTexture.Height(); // ...resize it to the chain texture's length.

                projectileCenter += directionToPlayer;
                directionToPlayer = mountedCenter - projectileCenter;
                distanceToPlayer = directionToPlayer.Length();

                Color lightColor = Lighting.GetColor((int)projectileCenter.X / 16, (int)(projectileCenter.Y / 16f));
                Main.spriteBatch.Draw(chainTexture.Value, projectileCenter - Main.screenPosition, null, lightColor, rotationToPlayer, chainTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }

            return false;
        }
    }

    public class PermafrostHookIceBlockPlayer : ModPlayer
    {
        public bool IsEncasedInIce { get; set; }
    }
}
