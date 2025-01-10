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
    public class FlareBolt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 65;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 35;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item73 with
            {
                Pitch = 0.1f,
                PitchVariance = 0.1f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<FlareBoltProjectile>();

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 4);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Bookcases)
                .AddIngredient(ItemID.SpellTome, 1)
                .AddIngredient(ItemID.LivingFireBlock, 20)
                .AddIngredient<HavocSoul>(15)
                .Register();
        }
    }
}
