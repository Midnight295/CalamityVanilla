using Microsoft.Xna.Framework;
using System;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Dusts
{
    public class MagicIceDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, Main.rand.Next(2) * 8, 8, 8);
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            dust.alpha = 0;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            Color newColor = Color.Lerp(Color.White, Color.Turquoise, CVUtils.InverseLerp(0, 15, dust.alpha));
            float alpha = 1 - dust.alpha / 15f;
            alpha = MathF.Sqrt(MathF.Sin(alpha * MathHelper.Pi));
            return lightColor.MultiplyRGBA(newColor) * alpha * 1.2f;
        }

        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.95f;
            dust.position += dust.velocity;

            dust.scale *= 0.97f;

            dust.alpha++;
            if (dust.alpha > 15)
            {
                dust.active = false;
            }

            return false;
        }
    }
}
