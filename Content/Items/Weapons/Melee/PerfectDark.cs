using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class PerfectDark : ModItem
    {
        public override void SetDefaults()
        {
            Item.Size = new Vector2(32, 32);
            Item.damage = 45;
            Item.knockBack = 3f;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.PerfectDarkCloud>();
            Item.shootSpeed = 5;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int cloudAmount = Main.rand.Next(2, 5 + 1);
            for (int i = 0; i < cloudAmount; i++)
            {
                Projectile.NewProjectile
                (
                    source,
                    position,
                    velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.66f, 1.5f),
                    type,
                    (int)(damage * 0.5f),
                    knockback,
                    player.whoAmI,
                    ai0: Main.rand.Next(90, 150)
                );
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            for (int i = 0; i < 2; i++)
            {
                float pointProgress = Main.rand.NextFloat(0.6f);
                CVUtils.GetPointOnSwungItemPath(68f, 84f, 0.4f + pointProgress, player.GetAdjustedItemScale(Item), out var location, out var outwardDirection, player);
                Vector2 velocity = outwardDirection.RotatedBy(MathHelper.PiOver2 * player.direction * player.gravDir);

                float dustInterpolator = Utils.GetLerpValue(0.2f, 0.4f, pointProgress, true);
                float dustScale = MathHelper.Lerp(1.5f, 2.5f, dustInterpolator);
                Color dustColor = Color.Lerp(Color.White, Color.Black, dustInterpolator);
                int dustAlpha = 0;// (int)MathHelper.Lerp(100, 0, dustInterpolator);
                int dustType = DustID.Shadowflame;

                Dust dust = Dust.NewDustPerfect(
                    location,
                    dustType,
                    velocity * 4f,
                    Alpha: dustAlpha,
                    newColor: dustColor,
                    Scale: dustScale
                );
                dust.noGravity = true;
            }
        }
    }
}
