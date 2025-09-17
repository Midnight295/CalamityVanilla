using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Pets
{
    public class MagicChisel : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.TwinsPetItem);
            Item.buffType = ModContent.BuffType<MagicChiselBuff>();
            Item.shoot = ModContent.ProjectileType<MagicChiselPet>();
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(Item.buffType, 3600);
            }
            return true;
        }
    }
}
