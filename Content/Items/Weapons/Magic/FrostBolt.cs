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
    public class FrostBolt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 19;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 21;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item20 with
            {
                Pitch = 1f,
                PitchVariance = 0.1f,
                Volume = 0.7f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<FrostBoltProjectile>();

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2), type, damage, knockback, player.whoAmI,
                ai0: Main.rand.NextFloat(0.05f, 1f));

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Bookcases)
                .AddIngredient(ItemID.SpellTome, 1)
                .AddRecipeGroup("CalamityVanillaAnyIceBlock", 20)
                .AddIngredient<EleumSoul>(15)
                .Register();
        }
    }
}
