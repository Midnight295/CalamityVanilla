using CalamityVanilla.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Equipment.Accessories
{
    [AutoloadEquip(EquipType.Waist)]
    public class ParasiticBelt : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 5);
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife < player.statLifeMax2/2)
            {
                player.GetDamage(DamageClass.Generic) += 0.2f;
                player.GetModPlayer<ParasiticBeltImbue>().beltIchorImbue = true;
                player.MeleeEnchantActive = true; // MeleeEnchantActive indicates to other mods that a weapon imbue is active.
            }
        }
    }
}