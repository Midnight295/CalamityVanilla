using CalamityVanilla.Common.ItemDropRules.DropConditions;
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
            globalLoot.Add(ItemDropRule.ByCondition(new EleumSoulDropCondition(), ModContent.ItemType<EleumSoul>(), 5));
            globalLoot.Add(ItemDropRule.ByCondition(new HavocSoulDropCondition(), ModContent.ItemType<HavocSoul>(), 5));
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

                case NPCID.Drippler:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BouncingEyeball>(), 4, 15, 35));
                    break;
                case NPCID.DemonEye:
                case NPCID.DemonEye2:
                case NPCID.PurpleEye:
                case NPCID.PurpleEye2:
                case NPCID.GreenEye:
                case NPCID.GreenEye2:
                case NPCID.DialatedEye:
                case NPCID.DialatedEye2:
                case NPCID.SleepyEye:
                case NPCID.SleepyEye2:
                case NPCID.CataractEye:
                case NPCID.CataractEye2:
                case NPCID.DemonEyeOwl:
                case NPCID.DemonEyeSpaceship:
                    npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsBloodMoonAndNotFromStatue(), ModContent.ItemType<BouncingEyeball>(), 10, 5, 15));
                    break;
            }
        }
    }
}
