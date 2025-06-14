using CalamityVanilla.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged
{
    public class Crystaline : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToThrownWeapon(ModContent.ProjectileType<Projectiles.Ranged.Crystaline>(),23,12,true);
            Item.noUseGraphic = true;
            Item.damage = 12;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Anvils).AddIngredient(ItemID.ThrowingKnife, 150).AddIngredient(ItemID.FallenStar, 5).AddIngredient(ItemID.Diamond, 7).Register();
        }
    }
}
