﻿using CalamityVanilla.Common;
using CalamityVanilla.Content.Items.Consumable;
using CalamityVanilla.Content.Items.Equipment.Vanity;
using CalamityVanilla.Content.Items.Weapons.Magic;
using CalamityVanilla.Content.Items.Weapons.Melee;
using CalamityVanilla.Content.Items.Weapons.Ranged;
using CalamityVanilla.Content.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.NPCs.Bosses.HiveMind
{
    [AutoloadBossHead]
    public partial class HiveMind : ModNPC
    {
        public byte phase = 0;
        public Player target
        { get { return Main.player[NPC.target]; } }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (NPC.alpha > 64)
                return false;
            return base.CanHitPlayer(target, ref cooldownSlot);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(6, 6), Mod.Find<ModGore>("HiveMind" + $"{i}").Type);
                }
                for (int i = 0; i < 100; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.CorruptGibs);
                    d.velocity = Main.rand.NextVector2Circular(6, 6);
                    d.scale = Main.rand.NextFloat(1, 2);
                    d.noGravity = !Main.rand.NextBool(3);
                }
                for (int i = 0; i < 100; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Corruption);
                    d.velocity = Main.rand.NextVector2Circular(6, 6);
                    d.scale = Main.rand.NextFloat(1, 2);
                    d.noGravity = Main.rand.NextBool();
                }
            }
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to. Most NPCs are immune to Confused.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "CalamityVanilla/Assets/Textures/Bestiary/HiveMind_Preview",
                //PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 180;

            NPC.frameCounter++;
            if (NPC.frameCounter > 7)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        float hypnoMultiply = 0f;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Need to make the squish not look janky on platforms
            Asset<Texture2D> tex = TextureAssets.Npc[Type];
            int move = (int)MathHelper.SmoothStep(tex.Height() / 4f, 0, NPC.Opacity);

            float scaleTimer  = ((float)Math.Sin((Main.timeForVisualEffects     ) / 20f) + 1f) / 2.0f;
            float scaleTimer2 = ((float)Math.Sin((Main.timeForVisualEffects + 25) / 20f) + 1f) / 1.9f;
            float scaleTimer3 = ((float)Math.Sin((Main.timeForVisualEffects + 50) / 20f) + 1f) / 1.8f;
            float scaleTimer4 = ((float)Math.Sin((Main.timeForVisualEffects + 75) / 20f) + 1f) / 1.7f;
            float scaleTimer5 = ((float)Math.Sin((Main.timeForVisualEffects + 100) / 20f) + 1f) / 1.6f;

            spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, drawColor, NPC.Opacity) * NPC.Opacity,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity),
                SpriteEffects.None, 0
                );

            //Color hypnoColor = new Color(drawColor.R * 163f / 255f, drawColor.G * 73f / 255f, drawColor.B * 164f / 255f);
            Color hypnoColor = Color.Purple;

            
            if (phase == 3 & NPC.ai[1] > 10)
            {
                hypnoMultiply -= 0.025f;
            }

            if (phase == 3 & NPC.ai[0] > 20)
            {
                if (NPC.ai[1] < 10) hypnoMultiply = Math.Clamp(hypnoMultiply + 0.025f, 0f, 1f);
                spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, hypnoColor, NPC.Opacity) * NPC.Opacity * 0.35f * scaleTimer * hypnoMultiply,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity) * scaleTimer * hypnoMultiply,
                SpriteEffects.None, 0
                );

                spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, hypnoColor, NPC.Opacity) * NPC.Opacity * 0.35f * scaleTimer2 * hypnoMultiply,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity) * scaleTimer2 * hypnoMultiply,
                SpriteEffects.None, 0
                );

                spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, hypnoColor, NPC.Opacity) * NPC.Opacity * 0.35f * scaleTimer3 * hypnoMultiply,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity) * scaleTimer3 * hypnoMultiply,
                SpriteEffects.None, 0
                );

                spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, hypnoColor, NPC.Opacity) * NPC.Opacity * 0.35f * scaleTimer4 * hypnoMultiply,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity) * scaleTimer4 * hypnoMultiply,
                SpriteEffects.None, 0
                );

                spriteBatch.Draw
                (
                tex.Value,
                NPC.Center - Main.screenPosition + new Vector2(0, -9 + (move * MathHelper.SmoothStep(1.2f, 1, NPC.Opacity))),
                new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.frame.Width, NPC.frame.Height - move),
                Color.Lerp(Color.Black, hypnoColor, NPC.Opacity) * NPC.Opacity * 0.35f * scaleTimer5 * hypnoMultiply,
                NPC.rotation,
                NPC.frame.Size() / 2,
                Vector2.SmoothStep(new Vector2(0.2f, 1.6f), new Vector2(1), NPC.Opacity) * scaleTimer5 * hypnoMultiply,
                SpriteEffects.None, 0
                );
            }

            return false;
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

            // The order in which you add loot will appear as such in the Bestiary. To mirror vanilla boss order:
            // 1. Trophy
            // 2. Classic Mode ("not expert")
            // 3. Expert Mode (usually just the treasure bag)
            // 4. Master Mode (relic first, pet last, everything else in between)

            // Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HiveMindTrophy>(), 10));

            // All the Classic Mode drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HiveMindMask>(), 7));

            //marble tome is here just as a placeholder idk if i need to clarify that
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<MyceliumStaff>(), ModContent.ItemType<PerfectDark>()));

            // Finally add the leading rule
            npcLoot.Add(notExpertRule);

            // Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HiveMindBag>()));

            // ItemDropRule.MasterModeCommonDrop for the relic
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<HiveMindRelic>()));

            // ItemDropRule.MasterModeDropOnAllPlayers for the pet
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ItemID.BrainOfCthulhuPetItem, 4));
        }

        public override void OnKill()
        {
            BossDownedSystem.DownedHiveMind = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.EyeofCthulhu);

            NPC.lifeMax = 16000;
            NPC.defense = 30;

            NPC.aiStyle = -1;
            NPC.behindTiles = true;
            NPC.noGravity = false;
            phase = 0;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/HiveMind");
            NPC.Size = new Vector2(150);
            NPC.noTileCollide = false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement($"Mods.CalamityVanilla.NPCs.HiveMind.Bestiary")
                
            });
        }

        public override void AI()
        {
            switch (phase)
            {
                case 0:
                    Teleport();
                    break;
                case 1:
                    ShootSporeBombs();
                    break;
                case 2:
                    VineAttack();
                    break;
                case 3:
                    SpawnMinions();
                    break;
            }
        }
    }
}
