using CalamityVanilla.Content.Items.Weapons.Magic;
using CalamityVanilla.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityVanilla
{
    public partial class CalamityVanilla : Mod
    {
        public enum PacketType : byte
        {
            SpawnGraniteTomeBoltSparks
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PacketType packetType = (PacketType)reader.ReadByte();

            switch (packetType)
            {
                case PacketType.SpawnGraniteTomeBoltSparks:
                    Vector2 position = reader.ReadVector2();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)PacketType.SpawnGraniteTomeBoltSparks);
                        packet.WriteVector2(position);
                        packet.Send(-1, whoAmI);
                        break;
                    }
                    GraniteTomeBolt.SpawnParticles(position);
                    break;
                default:
                    break;
            }
        }
    }
}
