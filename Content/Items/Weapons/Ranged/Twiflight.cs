using CalamityVanilla.Common;
using CalamityVanilla.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Ranged
{
    public class Twiflight : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToBow(24, 12, true);
            Item.consumeAmmoOnFirstShotOnly = true;
            Item.damage = 20;
            Item.knockBack = 2;
            Item.UseSound = null;

            Item.UseSound = SoundID.Item102;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 4);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.DemonAltar)
                .AddIngredient(ItemID.BeesKnees)
                .AddIngredient(ItemID.HellwingBow)
                .AddIngredient(ItemID.BloodRainBow)
                .AddIngredient(ModContent.ItemType<Cloudfall>())
                .Register();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            PrettySparkleParticle sparkle = CVParticleOrchestrator.RequestPrettySparkleParticle();
            sparkle.LocalPosition = position + Vector2.Normalize(velocity) * 10;
            sparkle.Scale = new Vector2(6f, 1.3f);
            sparkle.Rotation = velocity.ToRotation();
            sparkle.DrawVerticalAxis = true;
            sparkle.ColorTint = new Color(1f, 0.5f, 0f, 0f);
            sparkle.FadeInEnd = 5;
            sparkle.FadeOutStart = 5;
            sparkle.FadeOutEnd = 20;
            Main.ParticleSystem_World_OverPlayers.Add(sparkle);
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(sparkle.LocalPosition, DustID.GemTopaz, new Vector2(0, Main.rand.NextFloat(-5, 5)).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                Dust d2 = Dust.NewDustPerfect(sparkle.LocalPosition, DustID.GemTopaz, new Vector2(Main.rand.NextFloat(-7, 7), 0).RotatedBy(sparkle.Rotation + Main.rand.NextFloat(-0.1f, 0.1f)));
                d2.scale = Main.rand.NextFloat(0.8f, 1.2f);
                d2.noGravity = true;
            }
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustDirect(sparkle.LocalPosition, 1,1, DustID.RainbowMk2);
                d.velocity = Main.rand.NextVector2Circular(6, 6);
                d.noGravity = true;
                d.color = Color.Lerp(new Color(0.6f, 1f, 1f), Color.MediumVioletRed, Main.rand.NextFloat()) with { A = 0 };
                d.fadeIn = Main.rand.NextFloat(0, 1.5f);
            }
            for (int i = -1; i < 2; i+= 2)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(i * 0.2f) * 0.6f,ModContent.ProjectileType<TwiflightFeather>(),damage,knockback,player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ModContent.ProjectileType<TwiflightFeather>();
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}
