using CalamityVanilla.Content.Items.Equipment.Vanity;
using CalamityVanilla.Content.Items.Weapons.Magic;
using CalamityVanilla.Content.Items.Weapons.Melee;
using CalamityVanilla.Content.Items.Weapons.Ranged;
using CalamityVanilla.Content.NPCs.Bosses.Cryogen;
using CalamityVanilla.Content.NPCs.Bosses.HiveMind;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Consumable
{
    // Basic code for a boss treasure bag
    public class HiveMindBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // This set is one that every boss bag should have.
            // It will create a glowing effect around the item when dropped in the world.
            // It will also let our boss bag drop dev armor..
            ItemID.Sets.BossBag[Type] = true;

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Purple;
            Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // We have to replicate the expert drops from MinionBossBody here

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<HiveMindMask>(), 7));
            //marble tome is here just as a placeholder idk if i need to clarify that
            itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<MyceliumStaff>(), ModContent.ItemType<MarbleTome>()));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<HiveMind>()));
        }
    }
}