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
    public class FrostBolt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 12;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.DD2_BookStaffCast with
            {
                Pitch = 0.5f,
                PitchVariance = 0.1f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ModContent.ProjectileType<FrostBoltProjectile>();

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 20);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI,
                ai0: Main.rand.NextFloat(-0.01f, 0.01f));

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Bookcases)
                .AddIngredient(ItemID.Book, 1)
                .AddRecipeGroup("CalamityVanillaAnyIceBlock", 10)
                .Register();
        }
    }
}
