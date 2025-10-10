using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityVanilla.Common.ItemDropRules.DropConditions;

public class EleumSoulDropCondition : IItemDropRuleCondition
{
    private static LocalizedText Description;

    public EleumSoulDropCondition()
    {
        Description ??= Language.GetOrRegister("Mods.CalamityVanilla.DropConditions.EleumSoul");
    }

    public bool CanDrop(DropAttemptInfo info)
    {
        NPC npc = info.npc;
        return Main.hardMode
            && !NPCID.Sets.CannotDropSouls[npc.type]
            && !npc.boss
            && !npc.friendly
            && npc.lifeMax > 1
            && npc.value >= 1f
            && info.player.position.Y / 16f > Main.worldSurface
            && info.player.ZoneSnow;
    }

    public bool CanShowItemDropInUI()
    {
        return true;
    }

    public string GetConditionDescription()
    {
        return Description.Value;
    }
}