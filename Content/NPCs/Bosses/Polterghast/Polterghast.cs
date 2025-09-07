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

namespace CalamityVanilla.Content.NPCs.Bosses.Polterghast
{
    [AutoloadBossHead]
    public partial class Polterghast : ModNPC
    {
        public byte phase = 0;
        public Player target => Main.player[NPC.target];

        private static Asset<Texture2D> spikeBall, spikeChain;
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
                for (int i = 0; i < 100; i++)
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit);
                    d.velocity = Main.rand.NextVector2Circular(8, 8);
                    d.scale = Main.rand.NextFloat(1.5f, 2.5f);
                    d.noGravity = true;
                }
            }
        }
        public override void SetStaticDefaults()
        {
            spikeBall = ModContent.Request<Texture2D>(Texture + "Spike");
            spikeChain = ModContent.Request<Texture2D>(Texture + "SpikeChain");

            NPCID.Sets.TrailCacheLength[Type] = 10; // The length of old position to be recorded
            NPCID.Sets.TrailingMode[Type] = 1; // The recording mode
            Main.npcFrameCount[Type] = 3;

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MinionBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to. Most NPCs are immune to Confused.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                //CustomTexturePath = "CalamityVanilla/Assets/Textures/Bestiary/HiveMind_Preview",
                PortraitScale = 0.7f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionXOverride = 10f,
                PortraitPositionYOverride = 10f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 9)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }


        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.EyeofCthulhu);

            NPC.lifeMax = 30000;
            NPC.defense = 30;

            NPC.aiStyle = -1;
            NPC.behindTiles = false;
            NPC.noGravity = true;
            phase = 0;
            Music = MusicID.Boss4;
            NPC.Size = new Vector2(90);
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath51;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement("The spirits of the dungeon have grown vengeful from their eternal purgatory, fusing into a bone-clad force of ghoulish destruction.")
            });
        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1f, 2f);
                d.velocity = NPC.velocity/2f;
            }


            NPC.direction = NPC.spriteDirection = NPC.Center.X < target.Center.X ? 1 : -1;
            NPC.TargetClosest();

            if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 targetPosition = target.Center;
                Vector2 position = NPC.Center;

                if (NPC.velocity.Length() > 12f)
                {
                    NPC.velocity *= 0.9f;
                }

                Vector2 direction = targetPosition - position;
                float distanceFromTarget = direction.Length();
                direction.Normalize();

                NPC.velocity += direction * distanceFromTarget / 400f;

                NPC.velocity.Y *= 0.94f;
                //NPC.velocity *= 0f;
            }
        }

        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCProjectiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Asset<Texture2D> tex = TextureAssets.Npc[Type];
            NPC.ai[1]++;

            SpriteEffects spriteEffects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(NPC.width, NPC.height);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                //Main.NewText(NPC.oldPos.Length);
                Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(-50f, -15);
                Color color = NPC.GetAlpha(drawColor) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) * 0.5f;
                spriteBatch.Draw(tex.Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale - k / (float)NPC.oldPos.Length / 3, spriteEffects, 0f);
            }

            for (int i = 1; i < 4; i++)
            {
                float spinSpeed = 35f; // the smaller the number, the faster the spike balls spin
                Vector2 ballDist = (Vector2.One * (130f + (float)Math.Sin(NPC.ai[1] / 50f) * 35f)).RotatedBy(NPC.ai[1] / spinSpeed).RotatedBy(MathHelper.TwoPi / 3f * i);
                Vector2 ballPos = NPC.Center - Main.screenPosition + ballDist;
                Rectangle ballRect = new Rectangle(0, 0, 52, 54);
                Rectangle chainRect = new Rectangle(0, 0, 14, 14);
                int chainCount = 5;
                Color lightColor;

                for (int j = 1; j < chainCount + 1; j++)
                {
                    Vector2 chainPos = ballDist / (chainCount + 1) * j + NPC.Center - Main.screenPosition;
                    lightColor = Lighting.GetColor((chainPos + Main.screenPosition).ToTileCoordinates());
                    spriteBatch.Draw(spikeChain.Value, chainPos, chainRect, lightColor, 0, spikeChain.Size() / 2, NPC.scale, SpriteEffects.None, 0);
                }
                lightColor = Lighting.GetColor((NPC.Center + ballDist).ToTileCoordinates());
                spriteBatch.Draw(spikeBall.Value, ballPos, ballRect, lightColor, 0, spikeBall.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            }

            //spriteBatch.Draw(backTexture.Value, NPC.Center - Main.screenPosition, smallFlake, Color.White * 0.7f, -NPC.rotation, smallFlake.Size() / 2, !ForTheWorthy ? 1f : 2f, SpriteEffects.None, 0);

            spriteBatch.Draw(tex.Value, NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);


            if (NPC.IsABestiaryIconDummy)
            {
                // This is required because we have NPC.alpha = 255, in the bestiary it would look transparent
                return true;
            }

            return false;
        }
    }
}
