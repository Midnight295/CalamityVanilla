using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

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
            Item.damage = 60;
            Item.knockBack = 6f;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 3, 10, 0);
            Item.shoot = ModContent.ProjectileType<Projectiles.Melee.TheSnowman>();
            Item.shootSpeed = 15.9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
    }
}
