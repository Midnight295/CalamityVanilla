using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Melee
{
    public class ForsakenSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(54, 54);
            Item.damage = 67;
            Item.knockBack = 6f;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.Pink;
            //Item.value = Item.sellPrice(0, 2, 10, 0);
            Item.shoot = ModContent.ProjectileType<ForsakenSaberBoulder>();
            Item.shootSpeed = 19;
        }
    }

    public class ForsakenSaberBoulder : ModProjectile
    {
        public int lerp;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }
        public override void OnKill(int timeLeft)
        {
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, new Vector2(1, 0), 11, 0.6f);
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, new Vector2(-1, 0), 11, 0.6f);
            SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
            for (int i = -2; i < 3; i++)
            {
                Vector2 pos = new Vector2(0, Projectile.Center.Y);
                float rot = Main.rand.NextFloat(-1, 1);
                int style = 2;
                if (i == 0) // middle pillar
                {
                    pos = Projectile.Center;
                    rot = Main.rand.NextFloat(-0.3f, 0.3f);
                    style = 1;
                }
                else if (i == -1) // left pillar
                {
                    pos.X = Projectile.Center.X + Projectile.Size.X * i;
                    rot = Main.rand.NextFloat(0f, -0.4f);
                }
                else if (i == -2) // farthest left pillar
                {
                    pos.X = Projectile.Center.X + Projectile.Size.X * i;
                    rot = Main.rand.NextFloat(0f, -0.4f);
                }
                else if (i == 1) // right pillar
                {
                    pos.X = Projectile.Center.X + Projectile.Size.X * i;
                    rot = Main.rand.NextFloat(0f, 0.4f);
                }
                else if (i == 2) // farthest right pillar
                {
                    pos.X = Projectile.Center.X + Projectile.Size.X * i;
                    rot = Main.rand.NextFloat(0f, 0.4f);
                }

                if (i == -2 || i == 2)
                {
                    style = 3;
                }

                Projectile.NewProjectile(Projectile.GetSource_Death(), pos, Vector2.Zero, ModContent.ProjectileType<ForsakenSaberPillar>(), Projectile.damage / 4, Projectile.knockBack, Main.myPlayer, rot, style, pos.Y);

            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 60;
            height = 60;
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity.Y += MathHelper.Lerp(0.15f, 0.45f, lerp++ * 0.07f);
            Projectile.velocity.X *= 0.98f;
            Projectile.rotation += 0.21f;

            if (Projectile.Center.Y <= player.Center.Y + 10)
            {
                Projectile.tileCollide = false;
            }
            else
                Projectile.tileCollide = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
            {
                Color color = Color.Yellow;
                color *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
                Rectangle rect = new(0, 0, 32, 32);
                Vector2 pos = Projectile.Center - Projectile.Size + Projectile.velocity;
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.oldPos[i] + (Projectile.Size / 2) - Main.screenPosition, rect, color, Projectile.oldRot[i], rect.Size() / 2f, Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            }
            for (int j = 0; j < 12; j++)
            {
                Vector2 afterimageOffset = (MathHelper.TwoPi * j / 12).ToRotationVector2() * 2f;
                Color glowColor = Color.Orange;
                Rectangle rect = new(0, 0, 32, 32);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition + afterimageOffset, rect, Projectile.GetAlpha(glowColor), Projectile.rotation, rect.Size() / 2, Projectile.scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            return base.PreDraw(ref lightColor);
        }
    }

    public class ForsakenSaberPillar : ModProjectile
    {
        public Texture2D texture;
        public float mult;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
        }
        public override void OnSpawn(IEntitySource source)
        {
            /*Vector2 pos = new(0, Projectile.ai[2]);
            if (Projectile.ai[1] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + Projectile.Size.X * -1, pos.Y), Vector2.Zero, ModContent.ProjectileType<ForsakenSaberPillar>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(0f, -0.4f), 2, pos.Y);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + Projectile.Size.X, pos.Y), Vector2.Zero, ModContent.ProjectileType<ForsakenSaberPillar>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(0f, 0.4f), 2, pos.Y);
            }
            if (Projectile.ai[1] == 2)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + Projectile.Size.X * -1.1f, pos.Y), Vector2.Zero, ModContent.ProjectileType<ForsakenSaberPillar>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(0f, -0.4f), 3, pos.Y);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X + Projectile.Size.X * 1.1f, pos.Y), Vector2.Zero, ModContent.ProjectileType<ForsakenSaberPillar>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(0f, 0.4f), 3, pos.Y);
            }*/
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.ai[0];
            Projectile.position.Y = Projectile.ai[2] - Projectile.height;           
        }
        public int lerp;

        public override bool PreDraw(ref Color lightColor)
        {
            switch (Projectile.ai[1])
            {
                case 1:
                    {
                        texture = TextureAssets.Projectile[Type].Value;
                        Projectile.width = texture.Width;
                        Projectile.height = texture.Height;
                        mult = 1.4f;
                    }
                    break;
                case 2:
                    {
                        texture = ModContent.Request<Texture2D>("CalamityVanilla/Content/Items/Weapons/Melee/ForsakenSaberPillarMedium", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                        Projectile.width = texture.Width;
                        Projectile.height = texture.Height;
                        mult = 1.1f;
                    }
                    break;

                case 3:
                    {
                        texture = ModContent.Request<Texture2D>("CalamityVanilla/Content/Items/Weapons/Melee/ForsakenSaberPillarLarge", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                        Projectile.width = texture.Width;
                        Projectile.height = texture.Height;
                        mult = 1;
                    }
                    break;

            }
            Vector2 pos = Projectile.Center - Main.screenPosition;       
            Rectangle rect = texture.Frame();
            float height = MathHelper.Lerp(0f, 1, ++lerp * 0.07f);
            height *= mult;
            if (height >= 1)
                height = 1;
            
            pos.Y -= Projectile.Size.Y * height;
            Vector2 scale = new(1, 1);
            Main.EntitySpriteDraw(texture, pos + new Vector2(0, Projectile.Size.Y), rect, Projectile.GetAlpha(lightColor), Projectile.rotation, rect.Size() / 2, scale, SpriteEffects.None); 
            return false;
        }
    }
}
