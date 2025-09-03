using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged.Throwing
{
    public class CursedDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.DefaultToThrownWeapon(ModContent.ProjectileType<Projectiles.Ranged.CursedDaggerProjectile>(), 15, 15, true);
            Item.noUseGraphic = true;
            Item.damage = 30;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item1;
            Item.value = 12;
        }
        public override void AddRecipes()
        {
            CreateRecipe(66).AddTile(TileID.Anvils).AddIngredient(ItemID.RottenChunk, 1).AddIngredient(ItemID.CursedFlame, 1).Register();
        }
    }
}
