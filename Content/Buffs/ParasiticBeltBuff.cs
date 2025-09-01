using Terraria;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Buffs
{
    public class ParasiticBeltBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}