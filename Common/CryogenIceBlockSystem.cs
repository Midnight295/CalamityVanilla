using CalamityVanilla.Content.NPCs.Bosses.Cryogen;
using CalamityVanilla.Content.Projectiles.Hostile;
using CalamityVanilla.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityVanilla.Common
{
    public struct CryogenIceData
    {
        public ushort X;
        public ushort Y;
        public ushort Time;

        public CryogenIceData(ushort x, ushort y, ushort time)
        {
            X = x;
            Y = y;
            Time = time;
        }
        public CryogenIceData(int x, int y, int time)
        {
            X = (ushort)x;
            Y = (ushort)y;
            Time = (ushort)time;
        }
    }
    public class CryogenIceBlockSystem : ModSystem
    {
        public const int DEFAULT_ICE_TIMER = 2400;
        public static List<CryogenIceData> CryogenIceBlocks = new List<CryogenIceData>();
        public override void OnWorldUnload()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            for (int i = 0; i < CryogenIceBlocks.Count; i++)
            {
                if (Main.tile[CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y].TileType == ModContent.TileType<CryogenIceTile>())
                {
                    WorldGen.KillTile(CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y, false, false, true);
                }
            }
            CryogenIceBlocks.Clear();
        }
        public override void PostUpdateWorld()
        {
            bool cryogenIsREAL = false;
            foreach(var npc in Main.ActiveNPCs)
            {
                if(npc.type == ModContent.NPCType<Cryogen>())
                {
                    cryogenIsREAL = true;
                    break;
                }
            }
            for(int i = 0; i < CryogenIceBlocks.Count; i++)
            {
                CryogenIceBlocks[i] = new CryogenIceData(CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y, (ushort)(CryogenIceBlocks[i].Time - 1));
                if (Main.tile[CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y].TileType != ModContent.TileType<CryogenIceTile>())
                {
                    CryogenIceBlocks.RemoveAt(i);
                    return;
                }
                if (CryogenIceBlocks[i].Time < 0 || (!cryogenIsREAL && Main.rand.NextBool(10)))
                {
                    WorldGen.KillTile(CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y,false,false,true);
                    NetMessage.SendTileSquare(-1, CryogenIceBlocks[i].X, CryogenIceBlocks[i].Y);
                    CryogenIceBlocks.RemoveAt(i);
                }
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["CalamityVanilla:IceBlocks"] = CryogenIceBlocks;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("CalamityVanilla:IceBlocks"))
            {
                CryogenIceBlocks = tag.Get<List<CryogenIceData>>("CalamityVanilla:IceBlocks");
            }
        }
    }
}
