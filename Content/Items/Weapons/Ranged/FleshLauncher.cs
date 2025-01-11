using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityVanilla.Content.Projectiles.Ranged;

namespace CalamityVanilla.Content.Items.Weapons.Ranged
{
    public class FleshLauncher : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(ProjectileID.PurificationPowder, ItemID.Grenade, singleShotTime: 50, shotVelocity: 4f, hasAutoReuse: true);
            Item.width = 62;
            Item.height = 30;
            Item.damage = 10;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.buyPrice(gold: 7);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.Grenade)
            {
                type = ModContent.ProjectileType<Fleshnade>();
            }
            else if (type == ProjectileID.StickyGrenade)
            {
                type = ModContent.ProjectileType<StickyFleshnade>();
            }
            else if (type == ProjectileID.BouncyGrenade)
            {
                type = ModContent.ProjectileType<BouncyFleshnade>();
            }
            else if (type == ProjectileID.Beenade)
            {
                type = ModContent.ProjectileType<Honeycombnade>();
            }
            //position += new Vector2(0, MathHelper.SmoothStep(-10, 10, player.itemAnimation / (float)player.itemAnimationMax)).RotatedBy(velocity.ToRotation());
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
    }
}
