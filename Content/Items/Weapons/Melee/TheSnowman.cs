using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class TheSnowman : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(30, 30);
            Item.damage = 25;
            Item.knockBack = 6f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.TheSnowman>();
            Item.shootSpeed = 11;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
    }
}
