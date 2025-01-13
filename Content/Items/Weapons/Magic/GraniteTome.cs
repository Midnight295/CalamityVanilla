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
    public class GraniteTome : ModItem
    {
        public static readonly SoundStyle UseSound = new("CalamityVanilla/Assets/Sounds/ItemElectricZap");

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 4;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = UseSound with
            {
                Pitch = -0.3f,
                PitchVariance = 0.2f,
                Volume = 0.1f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.shoot = ModContent.ProjectileType<GraniteTomeBolt>();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 30);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.1), type, damage, knockback, player.whoAmI, 0, Main.rand.NextBool() ? 1 : -1);

            return false;
        }
    }
}
