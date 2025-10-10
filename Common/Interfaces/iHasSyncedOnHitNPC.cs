using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityVanilla.Common.Interfaces;

/// <summary>
/// Only usable on Items and Projectiles
/// </summary>
public interface iHasSyncedOnHitNPC
{
    public void SyncedOnHitNPC(Player player, NPC target, int damageDone, bool crit, int hitDirection);
}
public class SyncedOnHitNPCItem : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return lateInstantiation && entity.ModItem is iHasSyncedOnHitNPC;
    }
    public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (item.ModItem is iHasSyncedOnHitNPC i)
        {
            i.SyncedOnHitNPC(player, target, damageDone, hit.Crit, hit.HitDirection);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)CalamityVanilla.PacketType.SyncedOnHitNPC);
                packet.Write((byte)player.whoAmI);
                packet.Write(target.whoAmI);
                packet.Write(damageDone);
                packet.WriteFlags(hit.Crit, hit.HitDirection == 1);
                packet.Send(ignoreClient: player.whoAmI);
            }
        }
    }
    internal static void RecieveOnHitNPC(BinaryReader reader, int whoAmI)
    {
        Player p = Main.player[reader.ReadByte()];
        NPC n = Main.npc[reader.ReadInt32()];
        int dd = reader.ReadInt32();
        bool crit;
        bool rightHit;
        reader.ReadFlags(out crit, out rightHit);
        if (p.HeldItem.ModItem is iHasSyncedOnHitNPC i)
            i.SyncedOnHitNPC(p, n, dd, crit, rightHit ? 1 : -1);
    }
}