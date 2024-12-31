using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityVanilla.Content.Projectiles.Ranged;

namespace CalamityVanilla.Content.Items.Weapons.Ranged
{
    public class HoarfrostBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToBow(15, 12, true);
            Item.useTime = 3;
            Item.reuseDelay = 10;
            Item.damage = 40;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.consumeAmmoOnFirstShotOnly = true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<MistArrow>();
            }
            position += new Vector2(0, MathHelper.SmoothStep(-10,10, player.itemAnimation / (float)player.itemAnimationMax)).RotatedBy(velocity.ToRotation());
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2,0);
        }
    }
}
