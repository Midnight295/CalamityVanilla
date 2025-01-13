using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Magic
{
    public class MarbleTomeShield : ModProjectile
    {
        public const float DefaultDistance = 15f;

        private ref float Distance => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.timeLeft = 10;
            Projectile.penetrate = 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 10;

            if (Main.myPlayer == Projectile.owner)
            {
                float dist = MathHelper.Lerp(Distance, player.channel ? DefaultDistance : 0, 0.25f);
                if (Distance != dist)
                    Projectile.netUpdate = true;
                Distance = dist;
                if (Distance < 0.01) // lol floating point
                {
                    Projectile.Kill();
                    return;
                }
                float holdoutDistance = Distance * Projectile.scale;

                Vector2 aim = Vector2.Normalize(Main.MouseWorld - player.Center);
                if (aim.HasNaNs())
                {
                    aim = -Vector2.UnitY;
                }
                aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.25f));

                Vector2 holdoutOffset = aim * holdoutDistance;
                if (aim != Projectile.velocity)
                    Projectile.netUpdate = true;
                Projectile.velocity = holdoutOffset;
            }

            if (Projectile.velocity.X > 0f)
                player.ChangeDir(1);
            else if (Projectile.velocity.X < 0f)
                player.ChangeDir(-1);
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = player.Center;
            Projectile.rotation = Projectile.velocity.ToRotation();
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            }
            Projectile.oldPos[0] = Projectile.position - player.position;
            Projectile.oldRot[0] = Projectile.rotation;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = lightColor * Utils.GetLerpValue(0, DefaultDistance * 0.5f, Distance, true);
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);

            return false;
        }
    }
}
