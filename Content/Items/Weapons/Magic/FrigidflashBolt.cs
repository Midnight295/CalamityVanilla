using CalamityVanilla.Content.Items.Material;
using CalamityVanilla.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Magic
{
    public class FrigidflashBolt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 49;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item20 with
            {
                Pitch = 1f,
                Volume = 0.85f,
                PitchVariance = 0.5f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 6);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Main.rand.Next(new int[] { ModContent.ProjectileType<FrigidflashColdBoltProjectile>(), ModContent.ProjectileType<FrigidflashHotBoltProjectile>() });
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2), type, damage, knockback, player.whoAmI,
                ai0: Main.rand.NextFloat(0.1f, 3f));

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Bookcases)
                .AddIngredient<FrostBolt>()
                .AddIngredient<FlareBolt>()
                .AddIngredient(ItemID.Ectoplasm, 5)
                .Register();
        }
    }
}
