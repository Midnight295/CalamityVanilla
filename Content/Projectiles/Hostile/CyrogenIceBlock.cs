using CalamityVanilla.Common;
using CalamityVanilla.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Projectiles.Hostile
{
    public class CryogenIceBlock : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.QuickDefaults(true, 32);
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.direction * 0.1f;
            if (Projectile.Center.Distance(new Vector2(Projectile.ai[0], Projectile.ai[1])) < Projectile.velocity.Length())
            {
                Projectile.Kill();
            }
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod);
            d.velocity = Projectile.velocity;
            d.noGravity = true;
            d.scale = 1.5f;
        }
        public override void OnKill(int timeLeft)
        {
            Point placePos = Projectile.Center.ToTileCoordinates();
            //WorldGen.PlaceTile(placePos.X, placePos.Y, TileID.MagicalIceBlock);
            SoundEngine.PlaySound(SoundID.NPCDeath15, Projectile.position);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int squaresize = Main.rand.Next(1, 3);

                for (int x = -squaresize; x <= squaresize; x++)
                {
                    for (int y = -squaresize; y <= squaresize; y++)
                    {
                        if (!Main.rand.NextBool(16))
                        {
                            WorldGen.PlaceTile(placePos.X + x, placePos.Y + y, ModContent.TileType<CryogenIceTile>(), plr: Main.myPlayer);
                            CryogenIceBlockSystem.CryogenIceBlocks.Add(new Vector3(placePos.X + x, placePos.Y + y, (int)Projectile.ai[2] + Main.rand.Next(0, 60)));
                        }
                    }
                }
                NetMessage.SendTileSquare(-squaresize, placePos.X - squaresize, placePos.Y - 1, (squaresize * 2) + 1, (squaresize * 2) + 1);
            }
        }
    }
}
