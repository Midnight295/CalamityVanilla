using CalamityVanilla.Content.Items.Material;
using CalamityVanilla.Content.Projectiles.Melee;
using Microsoft.Build.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class CatastropheClaymore : ModItem
    {
        private static Asset<Texture2D> Glow;
        public override void SetStaticDefaults()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void SetDefaults()
        {
            Item.DefaultToSword(65, 20, 4);
            Item.shoot = ModContent.ProjectileType<CatastropheClaymoreBall>();
            Item.shootSpeed = 6;
            Item.value = Item.sellPrice(0, 10);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(Glow.Value, Item.Bottom - Main.screenPosition - new Vector2(0,Glow.Height() / 2), null, Color.White, rotation, Glow.Size() / 2, scale, SpriteEffects.None, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.MythrilAnvil)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.CursedFlame, 10)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ModContent.ItemType<EleumSoul>(), 5)
                .AddIngredient(ModContent.ItemType<HavocSoul>(), 5).Register();

            CreateRecipe().AddTile(TileID.MythrilAnvil)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Ichor, 10)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ModContent.ItemType<EleumSoul>(), 5)
                .AddIngredient(ModContent.ItemType<HavocSoul>(), 5).Register();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //int dust = DustID.Torch;
            //switch (Main.rand.Next(5))
            //{
            //    case 1:
            //        dust = DustID.IceTorch;
            //        break;
            //    case 2:
            //        dust = DustID.IchorTorch;
            //        break;
            //    case 3:
            //        dust = DustID.CursedTorch;
            //        break;
            //    case 4:
            //        dust = DustID.Shadowflame;
            //        break;
            //}

            //for (int i = 0; i < 2; i++)
            //{
            //    CVUtils.GetPointOnSwungItemPath(70f, 72f, 0.4f + Main.rand.NextFloat(0.8f), Item.scale, out var location2, out var outwardDirection2, player);
            //    Vector2 vector2 = outwardDirection2.RotatedBy((float)Math.PI / 2f * (float)player.direction * player.gravDir);

            //    Dust d = Dust.NewDustPerfect(location2, dust, new Vector2(player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f), 140, default(Color), 1.5f);
            //    d.noGravity = true;
            //    d.velocity *= 0.25f;
            //    d.velocity += vector2 * 5f;
            //}
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(1.5f, 2f), type, damage, knockback, player.whoAmI, Main.rand.Next(8));
            }
            return false;
        }
    }
    public class CatastropheClaymoreGlow : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(Terraria.DataStructures.PlayerDrawLayers.HeldItem);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<CatastropheClaymore>() && drawInfo.drawPlayer.ItemAnimationActive;
        }
        private static Asset<Texture2D> texture;
        public override void Load()
        {
            texture = ModContent.Request<Texture2D>("CalamityVanilla/Content/Items/Weapons/Melee/CatastropheClaymore_Overlay");
        }
        private void drawSword(ref PlayerDrawSet drawInfo, Color color, int frame)
        {
            Vector2 basePosition = drawInfo.drawPlayer.itemLocation - Main.screenPosition;
            basePosition = new Vector2((int)basePosition.X, (int)basePosition.Y) + (drawInfo.drawPlayer.RotatedRelativePoint(drawInfo.drawPlayer.Center) - drawInfo.drawPlayer.Center);
            Item heldItem = drawInfo.drawPlayer.HeldItem;

            DrawData swingDraw = new DrawData(
            texture.Value, // texture
            basePosition, // position
            new Rectangle(0, texture.Height() / 3 * frame, texture.Width(), texture.Height() / 3), // texture coords
            color, // color (wow really!?)
            drawInfo.drawPlayer.itemRotation,  // rotation
            new Vector2(drawInfo.drawPlayer.direction == -1 ? texture.Value.Width : 0, // origin X
            drawInfo.drawPlayer.gravDir == 1 ? texture.Value.Height / 3 : 0), // origin Y
            drawInfo.drawPlayer.GetAdjustedItemScale(heldItem), // scale
            drawInfo.itemEffect // sprite effects
            );

            drawInfo.DrawDataCache.Add(swingDraw);
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0)
                return;

            drawSword(ref drawInfo, Color.LightGray, 0);
            drawSword(ref drawInfo, new Color(Main.DiscoR,Main.DiscoG,Main.DiscoB,0) * 0.8f, 1);
            drawSword(ref drawInfo, new Color(255 - Main.DiscoR, 255 - Main.DiscoG, 255 - Main.DiscoB, 0) * 0.8f, 2);
        }
    }
}
