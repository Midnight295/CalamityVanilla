﻿using CalamityVanilla.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class PrismAnimation : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<PrismPiercer>() && drawInfo.drawPlayer.ItemAnimationActive;
        }
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            int frame = 0;

            if (drawInfo.drawPlayer.HeldItem.ModItem is PrismPiercer p)
            {
                if (p.Shattered)
                    frame = 1;
            }
            Asset<Texture2D> texture = TextureAssets.Item[drawInfo.heldItem.type];
            Vector2 basePosition = drawInfo.drawPlayer.itemLocation - Main.screenPosition;
            basePosition = new Vector2((int)basePosition.X, (int)basePosition.Y) + (drawInfo.drawPlayer.RotatedRelativePoint(drawInfo.drawPlayer.Center) - drawInfo.drawPlayer.Center);
            Item heldItem = drawInfo.drawPlayer.HeldItem;

            DrawData swingDraw = new DrawData(
            texture.Value, // texture
            basePosition, // position
            new Rectangle(0, texture.Height() / 2 * frame, texture.Width(), texture.Height() / 2), // texture coords
            drawInfo.drawPlayer.HeldItem.GetAlpha(Color.White), // color (wow really!?)
            drawInfo.drawPlayer.itemRotation,  // rotation
            new Vector2(drawInfo.drawPlayer.direction == -1 ? texture.Value.Width : 0, // origin X
            drawInfo.drawPlayer.gravDir == 1 ? texture.Value.Height / 2 : 0), // origin Y
            drawInfo.drawPlayer.GetAdjustedItemScale(heldItem), // scale
            drawInfo.itemEffect // sprite effects
            );
            drawInfo.DrawDataCache.Add(swingDraw);
            drawInfo.ItemLocation = Vector2.Zero;
        }
    }
    public class PrismPiercer : ModItem
    {
        public bool Shattered = false;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0.65f);
        }
        public override void SetDefaults()
        {
            Item.DefaultToSword(68, 20, 6);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<PrismPiercerStar>();
            Item.shootSpeed = 25;
            Item.scale = 1.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddTile(TileID.MythrilAnvil).AddIngredient(ModContent.ItemType<WindowPain>()).AddIngredient(ItemID.Starfury).AddIngredient(ItemID.Ectoplasm, 15).Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pointPoisition = new Vector2(position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -player.direction) + ((float)Main.mouseX + Main.screenPosition.X - position.X), player.MountedCenter.Y - 600f);
            new Vector2(100f, 0f);
            Vector2 mouseWorld2 = Main.MouseWorld;
            Vector2 vec = mouseWorld2;
            Vector2 vector60 = (pointPoisition - mouseWorld2).SafeNormalize(new Vector2(0f, -1f));
            while (vec.Y > pointPoisition.Y && WorldGen.SolidTile(vec.ToTileCoordinates()))
            {
                vec += vector60 * 16f;
            }

            Projectile.NewProjectile(source, pointPoisition, pointPoisition.DirectionTo(Main.MouseWorld) * Item.shootSpeed, type, (int)(damage * 1.5f), 0, player.whoAmI, 0f, vec.Y);
            return false;
        }

        public override void SetStaticDefaults()
        {
            DrawAnimationVertical animation = new DrawAnimationVertical(-1, 2, false);
            animation.NotActuallyAnimating = true;
            Main.RegisterItemAnimation(Type, animation);
        }
        public override bool? UseItem(Player player)
        {
            if (player.ItemAnimationJustStarted)
                Shattered = false;
            return base.UseItem(player);
        }
        public override bool? CanHitNPC(Player player, NPC target)
        {
            if (player.ItemAnimationJustStarted)
                return false;
            return base.CanHitNPC(player, target);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Shattered)
            {
                Shattered = true;
                SoundEngine.PlaySound(SoundID.Shatter with { PitchVariance = 0.3f }, player.position);
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, new Vector2(player.direction * Main.rand.NextFloat(3, 9), Main.rand.NextFloat(-6, -2)), ModContent.ProjectileType<PrismPiercerShard>(), Item.damage / 2, Item.knockBack / 3, player.whoAmI, target.whoAmI);
                }
                for (int i = 0; i < 20; i++)
                {
                    Dust d = Dust.NewDustPerfect(player.Center + new Vector2(player.direction * 17, 0) + Main.rand.NextVector2Circular(24, 12), DustID.Glass, new Vector2(player.direction * Main.rand.NextFloat(1, 3), Main.rand.NextFloat(-3, -1)));
                    d.noGravity = Main.rand.NextBool();
                }
            }
        }
    }
    public class PrismPiercerShard : ModProjectile
    {
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0.65f);
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.frame = Projectile.whoAmI % 4;
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            Projectile.ai[1]++;
            NPC n = Main.npc[(int)Projectile.ai[0]];
            if (!n.active)
            {
                NPC n2 = Projectile.FindTargetWithinRange(800);
                if (n2 != null)
                    Projectile.ai[0] = n2.whoAmI;
                else
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[1] > 50 && !Projectile.Hitbox.Intersects(n.Hitbox))
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(n.Center) * 20, 0.03f);
            }
            else
            {
                Projectile.velocity.Y += 0.05f;
            }

            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SimpleColorableGlowyDust>(), Projectile.velocity.X, Projectile.velocity.Y, 0, Color.Lerp(Main.DiscoColor, Color.White, 0.5f) with { A = 0 });
                d.noGravity = true;
                d.velocity *= 0.1f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
    }
    public class PrismPiercerStarShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.QuickDefaults();
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.Opacity = Projectile.timeLeft / 30f;
            Projectile.velocity *= 0.95f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SimpleColorableGlowyDust>(), Projectile.velocity.X, Projectile.velocity.Y, 0, Main.DiscoColor with { A = 0 });
                d.noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
            {
                float multiply = (1 - (i / (float)ProjectileID.Sets.TrailCacheLength[Type]));
                Main.EntitySpriteDraw(tex.Value, Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2, null, Main.DiscoColor with { A = 0 } * multiply * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, multiply, SpriteEffects.None);
            }

            Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0f) * Projectile.Opacity * 3, Projectile.rotation, tex.Size() / 2, 0.8f, SpriteEffects.None);
            return false;
        }
    }
    public class PrismPiercerStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Starfury);
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SimpleColorableGlowyDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, Main.DiscoColor with { A = 0 });
                d.noGravity = true;
            }
            if (Projectile.Center.Y > Projectile.ai[1])
            {
                Projectile.tileCollide = true;
            }
            //if (Projectile.tileCollide && Projectile.penetrate == -1)
            //    Projectile.penetrate = 1;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for(int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.position, ModContent.DustType<SimpleColorableGlowyDust>(), Main.rand.NextVector2Circular(6,6), 0, Color.Lerp(Main.DiscoColor,Color.White,Main.rand.NextFloat(0.5f)) with { A = 0 });
                d.noGravity = true;
                //d.fadeIn = Main.rand.NextFloat(2);
            }

            //if (Main.myPlayer != Projectile.owner)
            //    return;

            //float rotation = Main.rand.NextFloat(-0.1f, 0.1f);

            //for(int i = 0; i < 5; i++)
            //{
            //    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, Main.rand.NextFloat(-12f,-9f)).RotatedBy((i * MathHelper.TwoPi / 5f) + rotation), ModContent.ProjectileType<PrismPiercerStarShard>(), Projectile.damage / 3, 0, Projectile.owner);
            //}
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 vector = Projectile.velocity;
            Color color2 = Main.DiscoColor * 0.1f;
            Vector2 spinningpoint = new Vector2(0f, -4f);
            float num = 0f;
            float t = vector.Length();
            float num2 = Utils.GetLerpValue(3f, 5f, t, clamped: true);

            Vector2 vector2 = Projectile.Center + vector;
            Texture2D value = TextureAssets.Projectile[Projectile.type].Value;
            _ = new Rectangle(0, 0, value.Width, value.Height).Size() / 2f;
            Texture2D value2 = TextureAssets.Extra[ExtrasID.FallingStar].Value;
            Rectangle value3 = value2.Frame();
            Vector2 origin2 = new Vector2(value3.Width / 2f, 10f);
            _ = Color.Cyan * 0.5f * num2;
            Vector2 vector3 = new Vector2(0f, Projectile.gfxOffY);
            float num5 = (float)Main.timeForVisualEffects / 60f;
            Vector2 vector4 = vector2 + vector * 0.5f;
            Color color3 = Color.White * 0.5f * num2;
            color3.A = 0;
            Color color4 = color2 * num2;
            color4.A = 0;
            Color color5 = color2 * num2;
            color5.A = 0;
            Color color6 = color2 * num2;
            color6.A = 0;
            float num6 = vector.ToRotation();
            Main.EntitySpriteDraw(value2, vector4 - Main.screenPosition + vector3 + spinningpoint.RotatedBy((float)Math.PI * 2f * num5), value3, color4, num6 + (float)Math.PI / 2f, origin2, 1.5f + num, SpriteEffects.None);
            Main.EntitySpriteDraw(value2, vector4 - Main.screenPosition + vector3 + spinningpoint.RotatedBy((float)Math.PI * 2f * num5 + (float)Math.PI * 2f / 3f), value3, color5, num6 + (float)Math.PI / 2f, origin2, 1.1f + num, SpriteEffects.None);
            Main.EntitySpriteDraw(value2, vector4 - Main.screenPosition + vector3 + spinningpoint.RotatedBy((float)Math.PI * 2f * num5 + 4.1887903f), value3, color6, num6 + (float)Math.PI / 2f, origin2, 1.3f + num, SpriteEffects.None);
            Vector2 vector5 = vector2 - vector * 0.5f;
            for (float num7 = 0f; num7 < 1f; num7 += 0.5f)
            {
                float num8 = num5 % 0.5f / 0.5f;
                num8 = (num8 + num7) % 1f;
                float num9 = num8 * 2f;
                if (num9 > 1f)
                    num9 = 2f - num9;

                Main.EntitySpriteDraw(value2, vector5 - Main.screenPosition + vector3, value3, color3 * num9, num6 + (float)Math.PI / 2f, origin2, 0.3f + num8 * 0.5f, SpriteEffects.None);
            }

            float rotation = Projectile.rotation + Projectile.localAI[1];
            _ = (float)Main.timeForVisualEffects / 240f;
            _ = Main.GlobalTimeWrappedHourly;
            float globalTimeWrappedHourly = Main.GlobalTimeWrappedHourly;
            globalTimeWrappedHourly %= 5f;
            globalTimeWrappedHourly /= 2.5f;
            if (globalTimeWrappedHourly >= 1f)
                globalTimeWrappedHourly = 2f - globalTimeWrappedHourly;
            globalTimeWrappedHourly = globalTimeWrappedHourly * 0.5f + 0.5f;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Texture2D value4 = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = value4.Frame(1, 1);
            Main.EntitySpriteDraw(origin: rectangle.Size() / 2f, texture: value4, position: position, sourceRectangle: rectangle, color: new Color(1f, 1f, 1f, 0.5f) * Projectile.Opacity, rotation: rotation, scale: Projectile.scale * 1.2f, effects: SpriteEffects.None);
            return false;
        }
    }
}
