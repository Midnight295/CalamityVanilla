using CalamityVanilla.Common.ItemDropRules.DropConditions;
using CalamityVanilla.Content.Items.Material;
using CalamityVanilla.Content.Items.Weapons.Magic;
using CalamityVanilla.Content.Items.Weapons.Ranged;
using CalamityVanilla.Content.Items.Weapons.Ranged.Throwing;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Common
{
    public class CVGlobalItem : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            switch (item.type)
            {
                case ItemID.WallOfFleshBossBag:
                    itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<FleshLauncher>(), ModContent.ItemType<PyrobatStaff>()));
                    break;
            }
        }
    }
}
