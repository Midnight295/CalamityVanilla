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
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item94 with
            {
                Pitch = 1f,
                PitchVariance = 0.3f,
                MaxInstances = 0,
            };
            Item.autoReuse = true;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<GraniteTomeBolt>();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 30);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2), type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}
