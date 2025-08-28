using CalamityVanilla.Content.Items.Material;
using CalamityVanilla.Content.Items.Weapons.Ranged;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Common
{
    public class CVGlobalNPC : GlobalNPC
    {
        public override void ModifyGlobalLoot(GlobalLoot globalLoot) //Drop Cryo Key Mold
        {
            globalLoot.Add(ItemDropRule.ByCondition(new Conditions.FrozenKeyCondition(), ModContent.ItemType<CryogenSummonMold>(), 100));
            globalLoot.Add(ItemDropRule.ByCondition(new Conditions.FrozenKeyCondition(), ModContent.ItemType<EleumSoul>(), 5));
            globalLoot.Add(ItemDropRule.ByCondition(new Conditions.LivingFlames(), ModContent.ItemType<HavocSoul>(), 5));
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.GoblinArcher:
                case NPCID.GoblinPeon:
                case NPCID.GoblinSorcerer:
                case NPCID.GoblinThief:
                case NPCID.GoblinWarrior:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoblinScrap>(), 4,1,2));
                    break;
                case NPCID.Vampire:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TheGothic>(), 50));
                    break;
            }
        }
    }
}
