using CalamityVanilla.Content.NPCs.Bosses.Cryogen;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Achievements;

public class CryogenKilled : ModAchievement
{
    public override void SetStaticDefaults()
    {
        AddNPCKilledCondition(ModContent.NPCType<Cryogen>());
    }

    // By default a ModAchievement will be placed at the end of the achievement ordering.
    // GetDefaultPosition is used to position a ModAchievement in relation to vanilla achievements.
    // Since MinionBoss is similar to Eye of Cthulhu, we place it after its achievement, "EYE_ON_YOU".
    public override Position GetDefaultPosition() => new Before("THE_GREAT_SOUTHERN_PLANTKILL");
}