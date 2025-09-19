using Terraria.ModLoader;

namespace CalamityVanilla.Common.Players
{
    public class PlayerStats : ModPlayer
    {
        public int TimeInWorld;

        public override void PreUpdate()
        {
            TimeInWorld++;
        }
    }
}
