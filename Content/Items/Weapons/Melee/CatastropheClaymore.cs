using CalamityVanilla.Content.Projectiles.Melee;
using Microsoft.Build.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class CatastropheClaymore : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToSword(75, 20, 4);
            Item.shoot = ModContent.ProjectileType<CatastropheClaymoreBall>();
            Item.shootSpeed = 6;
            Item.value = Item.sellPrice(0, 10);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.CursedFlame, 10).AddIngredient(ItemID.SoulofFright, 5).AddIngredient(ItemID.SoulofMight, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.SoulofNight, 5).AddIngredient(ItemID.SoulofLight, 5).Register();
            CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.Ichor, 10).AddIngredient(ItemID.SoulofFright, 5).AddIngredient(ItemID.SoulofMight, 5).AddIngredient(ItemID.SoulofSight, 5).AddIngredient(ItemID.SoulofNight, 5).AddIngredient(ItemID.SoulofLight, 5).Register();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dust = DustID.Torch;
            switch (Main.rand.Next(5))
            {
                case 1:
                    dust = DustID.IceTorch;
                    break;
                case 2:
                    dust = DustID.IchorTorch;
                    break;
                case 3:
                    dust = DustID.CursedTorch;
                    break;
                case 4:
                    dust = DustID.Shadowflame;
                    break;
            }

            for (int i = 0; i < 2; i++)
            {
                CVUtils.GetPointOnSwungItemPath(70f, 72f, 0.4f + Main.rand.NextFloat(0.8f), Item.scale, out var location2, out var outwardDirection2, player);
                Vector2 vector2 = outwardDirection2.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);

                Dust d = Dust.NewDustPerfect(location2, dust, new Vector2(player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f), 140, default(Color), 1.5f);
                d.noGravity = true;
                d.velocity *= 0.25f;
                d.velocity += vector2 * 5f;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(1,4); i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(1f, 1.5f), type, damage / 3, knockback, player.whoAmI, Main.rand.Next(5),Main.rand.NextFloat(30));
            }
            return false;
        }
    }
}
