using CalamityVanilla.Common;
using CalamityVanilla.Content.Projectiles.Hostile;
using CalamityVanilla.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.NPCs.Bosses.Cryogen
{
    public partial class Cryogen : ModNPC
    {
        private void FlyAndShoot_0()
        {
            NPC.ai[2]--;
            if (NPC.ai[0] <= 0)
            {
                NPC.velocity.Y += 0.3f;
                NPC.velocity += NPC.Center.DirectionTo(target.Center) * 0.4f;
                NPC.velocity = NPC.velocity.LengthClamp(4.5f, 0);
                if(NPC.life < NPC.lifeMax * _phase2HealthMultiplier)
                {
                    NPC.ai[0] = -30;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    phase = 2;
                }
                if (NPC.ai[2] <= 0) // Ice Pillar
                {
                    NPC.ai[2] = 60 * 30;
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 endPoint = target.Center + target.velocity * new Vector2(160,1);
                        for(int i = 0; i < 300; i++)
                        {
                            Point p = endPoint.ToTileCoordinates();
                            p.Y += i;
                            if (Main.tileSolid[Main.tile[p].TileType] && Main.tile[p].HasTile)
                            {
                                p.Y -= 3;
                                endPoint.Y = p.ToWorldCoordinates().Y;
                                break;
                            }
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromThis(),NPC.Center,NPC.Center,ModContent.ProjectileType<IcePillarSpawner>(),0,0,-1,endPoint.X,endPoint.Y);
                    }
                }
            }
            else
            {
                NPC.rotation += NPC.direction * 0.1f;
                NPC.velocity *= 0.99f;
                bool hitAnything = false;
                for (int x = NPC.Left.ToTileCoordinates().X; x < NPC.Right.ToTileCoordinates().X; x++) // Destroy ice blocks
                {
                    for (int y = NPC.Top.ToTileCoordinates().Y; y < NPC.Bottom.ToTileCoordinates().Y; y++)
                    {
                        if (Main.tile[x, y].TileType == ModContent.TileType<CryogenIceTile>())
                        {
                            hitAnything = true;
                            WorldGen.KillTile(x, y);
                            if (Main.rand.NextBool(6) && Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), new Point(x, y).ToWorldCoordinates(), new Point(x, y).ToWorldCoordinates().DirectionTo(target.Center).RotatedByRandom(0.1f) * 17, ModContent.ProjectileType<IceShrapnel>(), 12, 1, -1);
                            }
                        }
                    }
                }
                if (hitAnything)
                {
                    NPC.velocity *= 0.9f;
                    NPC.localAI[0] = 5;
                }
            }
            if (NPC.ai[0] == -31)
            {
                NPC.TargetClosest();
                if (NPC.ai[1] % 4 == 0)
                {
                    NPC.ai[0] = 60;
                }
            }
            if (NPC.ai[0] > -30 && NPC.ai[0] <= 0)
            {
                float multiplier = (30 + NPC.ai[0]) / 30f;
                NPC.rotation += NPC.direction * 0.3f * multiplier;
                NPC.velocity += NPC.Center.DirectionTo(target.Center) * -multiplier * 3;
            }

            if (NPC.ai[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack, NPC.Center);
                NPC.velocity = NPC.Center.DirectionTo(target.Center) * 24f;
            }
            else if (NPC.ai[0] == 60)
            {
                NPC.ai[1]++;
                NPC.ai[0] = NPC.ai[1] % 3 == 0? -200 : -120;
            }
            NPC.ai[0]++;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                switch(NPC.ai[1] % 3)
                {
                    case 0: // Ice blocks or do the slam thing instead
                        if (NPC.ai[0] is -190 or -180 or -170 or -100 or -90 or -80)
                        {

                            if (NPC.ai[0] == -190 && Main.rand.NextBool())
                            {
                                NPC.ai[0] = -60;
                                NPC.ai[2] = 0;
                                NPC.ai[1] = -1.5f;
                                phase = 1;
                                NPC.netUpdate = true;
                                return;
                            }
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 blockPlacement = (target.Center + target.velocity.LengthClamp(1000,5) * 40) + Main.rand.NextVector2Circular(16 * 4, 16 * 4);
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(blockPlacement) * 48, ModContent.ProjectileType<CryogenIceBlock>(), 0, 0, ai0: blockPlacement.X, ai1: blockPlacement.Y, ai2: CryogenIceBlockSystem.DEFAULT_ICE_TIMER);
                            }
                        }
                        break;
                    case 1: // Statues
                        if (NPC.ai[0] is -110 or -90 or - 70 or -50)
                        {
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, Main.rand.NextVector2CircularEdge(16,16), ModContent.ProjectileType<IceStatues>(), 40, 0,-1,target.whoAmI,NPC.whoAmI,-80);
                        }
                        break;
                    case 2: // Bombs
                        if (NPC.ai[0] == -110)
                        {
                            for (int i = -2; i <= 2; i++)
                            {
                                Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(target.Center).RotatedBy(0.2 * i) * (10 - Math.Abs(i * 2)), ModContent.ProjectileType<IceBomb>(), 30, 0, -1, Main.rand.Next(2),15);
                            }
                        }
                        break;
                }
            }
        }
        private void SlamAttack_1()
        {
            NPC.ai[0]++;
            if (NPC.ai[2] == 0)
            {
                if (NPC.ai[0] < 0)
                {
                    NPC.velocity.Y += 0.3f;
                    NPC.velocity += NPC.Center.DirectionTo(target.Center + new Vector2(0, -300)) * 1.4f;
                    NPC.velocity = NPC.velocity.LengthClamp(9, 0);
                }
                else
                {

                    NPC.velocity.X *= 0.96f;
                    NPC.ai[1] += 0.1f;
                    NPC.velocity.Y += NPC.ai[1];
                    NPC.velocity = NPC.velocity.LengthClamp(32, 0);

                    if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) && NPC.velocity.Y > 5)
                    {
                        NPC.ai[2] = 1;
                        NPC.ai[0] = 0;
                        NPC.velocity.Y = -20;
                        SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 15; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Bottom, Main.rand.NextVector2Circular(10,10) - new Vector2(0,15), ModContent.ProjectileType<IceChunks>(), 20, 1);
                            }
                        }
                    }
                }
            }
            else
            {
                NPC.velocity *= 0.99f;
                NPC.ai[0]++;
                if (NPC.ai[0] == 60)
                {
                    phase = 0;
                    NPC.ai[0] = -30;
                    NPC.ai[1] = 1;
                    NPC.ai[2] = 0;
                }
            }
        }
    }
}
