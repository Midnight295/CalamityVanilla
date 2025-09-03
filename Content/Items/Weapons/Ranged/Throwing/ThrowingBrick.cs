using CalamityVanilla.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged.Throwing
{
    public class ThrowingBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.DefaultToThrownWeapon(ModContent.ProjectileType<Projectiles.Ranged.ThrowingBrick>(), 30, 10, true);
            Item.noUseGraphic = true;
            Item.damage = 16;
            Item.knockBack = 7;
            Item.rare = ItemRarityID.White;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item1;
            Item.value = 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ItemID.ClayBlock, 1).Register();
            CreateRecipe(8).AddIngredient(ItemID.RedBrick, 1).Register();
        }
    }
}
