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
            Item.DefaultToThrownWeapon(ModContent.ProjectileType<Projectiles.Ranged.Crystaline>(),16,8,true);
            Item.noUseGraphic = true;
            Item.damage = 12;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = false;
            Item.maxStack = 1;
        }
        public override void HoldItem(Player player)
        {
            Main.NewText(player.stealth);
            player.GetModPlayer<CVModPlayer>().StealthEnabled = true;
            if (player.itemAnimation > 0)
            {
                player.stealthTimer = 15;
                if (player.stealth > 0f)
                {
                    player.stealth += 0.1f;
                }
            }
            else if (player.velocity.X > -0.1f && player.velocity.X < 0.1f && player.velocity.Y > -0.1f && player.velocity.Y < 0.1f && !player.mount.Active)
            {
                if (player.stealthTimer == 0 && player.stealth > 0f)
                {
                    player.stealth -= 0.02f;
                    if (player.stealth <= 0.0)
                    {
                        player.stealth = 0f;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, player.whoAmI);
                        }
                    }
                }
            }
            else
            {
                if (player.stealth > 0f)
                {
                    player.stealth += 0.1f;
                }
                if (player.mount.Active)
                {
                    player.stealth = 1f;
                }
            }
            if (player.stealth > 1f)
            {
                player.stealth = 1f;
            }
        }
    }
}
