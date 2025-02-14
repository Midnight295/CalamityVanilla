using CalamityVanilla.Content.Projectiles.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityVanilla.Content.Items.Weapons.Magic
{
    public class MarbleTome : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 4;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MarbleRock>();
            Item.shootSpeed = 14f;
            Item.channel = true;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1);
        }
    }
}
