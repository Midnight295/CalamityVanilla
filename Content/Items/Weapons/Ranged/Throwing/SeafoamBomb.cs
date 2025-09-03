using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged.Throwing
{
    public class SeafoamBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.DefaultToThrownWeapon(ModContent.ProjectileType<Projectiles.Ranged.SeafoamBomb>(), 15, 10, true);
            Item.noUseGraphic = true;
            Item.damage = 20;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.White;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item1;
            Item.value = 15;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.Anvils).AddIngredient(ItemID.Grenade).AddIngredient(ItemID.Coral).Register();
        }
    }
}
