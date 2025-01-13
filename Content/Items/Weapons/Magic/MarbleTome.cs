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
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 0f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MarbleTomeShield>();
            Item.shootSpeed = 4f;
            Item.channel = true;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1);
        }

        public override bool CanShoot(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }
}
