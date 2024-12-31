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
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Melee
{
    public class CatastropheClaymoreBall : ModProjectile
    {
        const byte FIRE = 0;
        const byte FROST = 1;
        const byte ICHOR = 2;
        const byte CURSE = 3;
        const byte SHADOW = 4;
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(false, 16);
        }
        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 30)
            {
                Projectile.scale *= 0.97f;
                Projectile.velocity.X *= 0.97f;
            }
            if (Projectile.scale < 0.5f)
            {
                Projectile.Kill();
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            Projectile.frameCounter++;
            Projectile.frame = (Projectile.frameCounter / 3) % 6;

            if (Projectile.ai[1] < 5)
                return;

            int dust = DustID.Torch;

            switch (Projectile.ai[0])
            {
                case FROST:
                    dust = DustID.IceTorch;
                    break;
                case ICHOR:
                    dust = DustID.IchorTorch;
                    break;
                case CURSE:
                    dust = DustID.CursedTorch;
                    break;
                case SHADOW:
                    dust = DustID.Shadowflame;
                    break;
            }

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dust);
            d.velocity *= 0.3f;
            d.noGravity = true;
            d.scale = Projectile.scale;
            d.alpha = Projectile.ai[0] == SHADOW ? 128 : 0;
        }
        public override void OnKill(int timeLeft)
        {
            int dust = DustID.Torch;
            switch (Projectile.ai[0])
            {
                case FROST:
                    dust = DustID.IceTorch;
                    break;
                case ICHOR:
                    dust = DustID.IchorTorch;
                    break;
                case CURSE:
                    dust = DustID.CursedTorch;
                    break;
                case SHADOW:
                    dust = DustID.Shadowflame;
                    break;
            }
            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dust);
                d.velocity *= Projectile.ai[0] == SHADOW ? 4f : 6f;
                d.noGravity = true;
                d.scale = 1.5f;
                d.alpha = Projectile.ai[0] == SHADOW ? 128 : 0;
            }
            Projectile.Explode(16 * 8);
            SoundEngine.PlaySound(SoundID.Item20 with { MaxInstances = 10, PitchVariance = 0.3f}, Projectile.position);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int debuff = BuffID.OnFire3;
            switch (Projectile.ai[0])
            {
                case FROST:
                    debuff = BuffID.Frostburn2;
                    break;
                case ICHOR:
                    debuff = BuffID.Ichor;
                    break;
                case CURSE:
                    debuff = BuffID.CursedInferno;
                    break;
                case SHADOW:
                    debuff = BuffID.ShadowFlame;
                    break;
            }
            target.AddBuff(debuff, 60 * 3);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];

            Color back = Color.OrangeRed;
            Color front = new Color(255,200,64,0);

            switch (Projectile.ai[0])
            {
                case FROST:
                    back = new Color(0,128,255);
                    front = new Color(180, 255, 255, 0);
                    break;
                case ICHOR:
                    back = new Color(128, 70, 0);
                    front = new Color(255, 255, 128, 0);
                    break;
                case CURSE:
                    back = new Color(0, 190, 10);
                    front = new Color(255, 255, 0, 0);
                    break;
                case SHADOW:
                    back = new Color(128, 0, 255);
                    front = new Color(255, 200, 255, 0);
                    break;
            }

            Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, tex.Frame(2, 6, 0, Projectile.frame), back * Projectile.Opacity, Projectile.rotation, new Vector2(24, 34),Projectile.scale,SpriteEffects.None);
            Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, tex.Frame(2, 6, 1, Projectile.frame), front * Projectile.Opacity, Projectile.rotation, new Vector2(24, 34), Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
