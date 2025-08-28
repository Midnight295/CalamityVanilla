using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityVanilla.Common.ItemDropRules.DropConditions
{
    public class HavocSoulDropCondition : IItemDropRuleCondition
    {
        private static LocalizedText Description;

        public HavocSoulDropCondition()
        {
            Description ??= Language.GetOrRegister("Mods.CalamityVanilla.DropConditions.HavocSoul");
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
                && info.player.ZoneUnderworldHeight;
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
}