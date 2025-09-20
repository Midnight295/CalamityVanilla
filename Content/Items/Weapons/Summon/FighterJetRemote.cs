using CalamityVanilla.Common.Players;
using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityVanilla.Content.Items.Weapons.Summon
{
    public class FighterJetRemote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.damage = 26;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0.25f;

            Item.shoot = ModContent.ProjectileType<FighterJetMinion>();
            Item.buffType = ModContent.BuffType<FighterJetBuff>();
            Item.shootSpeed = 1f;

            Item.UseSound = SoundID.Item37 with { Volume = 0.4f };
            Item.autoReuse = true;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }
    }

    public class FighterJetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<FighterJetMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }

    public class FighterJetMinion : ModProjectile
    {
        private enum State
        {
            Idle,
            Attack
        }

        public ref float AI_Timer => ref Projectile.ai[0];
        public ref float AI_State => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            DrawOriginOffsetY = 7;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        public Vector2 randPos = new Vector2(200, 0);

        public int AI_Shoot_Timer = 0;
        
        public int startAttackRange = 950;
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            float distFromOwner = Projectile.Center.Distance(owner.Center);
            if (!CheckAlive(owner)) return;
            PlayerStats stats = owner.GetModPlayer<PlayerStats>();
            float speed = 0.25f;
            float spaceFromOtherJets = 0.65f;

            Entity target = owner;

            float closestTargetDistance = startAttackRange;
            NPC targetNPC = null;
            // Prioritize the owner's minion attack target. (Right click or whip feature)
            if (Projectile.OwnerMinionAttackTargetNPC != null)
            {
                TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);
            }

            // If no minion attack target or if it was out of range, find the closest enemy to target.
            if (targetNPC == null)
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
                }
            }


            if (targetNPC == null) { AI_State = (float)State.Idle; }
            else { AI_State = (float)State.Attack; }

            switch (AI_State)
            {
                case (float)State.Idle:
                    target = owner;
                    break;

                case (float)State.Attack:
                    target = targetNPC;
                    speed = 0.45f;
                    spaceFromOtherJets = 1f;

                    if (Main.myPlayer == Projectile.owner)
                    {
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);

                        // short pause between bullets
                        if (AI_Shoot_Timer-- <= 0)
                        {
                            AI_Shoot_Timer = 7 * Main.rand.Next(7, 12);
                        }

                        // shoot bullets!!!!
                        if (AI_Shoot_Timer > 35)
                        {
                            if (AI_Timer % 7 == 0 && lineOfSight)
                            {
                                float shootSpeed = 8f;
                                Vector2 shootDir = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-5, 5)));
                                Vector2 shootVelocity = shootDir * shootSpeed;

                                Item item = ContentSamples.ItemsByType[ItemID.SDMG]; // used to automatically consume bullets with 66% chance not to consume ammo

                                owner.PickAmmo(item, out int projToShoot, out shootSpeed, out int damage, out float knockBack, out int usedAmmoItemId);


                                SoundEngine.PlaySound(SoundID.Item11 with { Volume = 0.7f }, Projectile.Center);
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X - 4f, Projectile.Center.Y), shootVelocity, projToShoot, Projectile.damage, 3, Projectile.owner);
                            }
                        }
                    }
                    break;
            }

            AI_Timer++;
            if (AI_Timer > 1)
            {
                float heightAboveTarget = 0f;
                float dist = Projectile.Center.Distance(target.Center + new Vector2(0, heightAboveTarget));
                Vector2 dir = Projectile.Center.DirectionTo(target.Center + new Vector2(0, heightAboveTarget));
                Vector2 distVector = Projectile.Center - target.Center + new Vector2(0, heightAboveTarget);

                // if too close or too fast, slow down
                if (dist < 400f || Projectile.velocity.LengthSquared() > 16f)
                {
                    Projectile.velocity *= 0.96f;
                }

                // move away from target if intersecting with target's hitbox
                if (Projectile.Hitbox.Intersects(target.Hitbox))
                {
                    Projectile.velocity += new Vector2(0.5f, 0).RotatedBy(Projectile.rotation);
                }

                // keep distance from other minions
                spaceFromOtherJets = 0.65f;

                // get average of surrounding jets' rotations so they all fly in similar directions
                Vector2 averageRotation = Vector2.Zero;
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<FighterJetMinion>() && projectile.identity != Projectile.identity && projectile.Hitbox.Intersects(Projectile.Hitbox))
                    {
                        Projectile.position += new Vector2(spaceFromOtherJets * Math.Sign(Projectile.Center.X - projectile.Center.X), spaceFromOtherJets * Math.Sign(Projectile.Center.Y - projectile.Center.Y));
                    }

                    if (projectile.type == ModContent.ProjectileType<FighterJetMinion>() && projectile.identity != Projectile.identity && Projectile.Center.Distance(projectile.Center) < 100f)
                    {
                        averageRotation += projectile.rotation.ToRotationVector2();

                        Projectile.rotation = averageRotation.ToRotation();
                        Projectile.velocity += speed * Vector2.UnitX.RotatedBy(Projectile.rotation) / 12f;
                    }
                }

                if (dist > 200f) { speed = 0.75f; }
                Projectile.velocity += speed * dir + target.velocity/30f;
                Projectile.rotation = (Projectile.velocity).ToRotation();

                if (dist > 2000f)
                {
                    Projectile.Center = owner.Center;
                }
            }

            // cool fire trail
            int dustAmount = 1;
            if (Projectile.velocity.Length() > 8f) dustAmount = 2;
            for (int i = 0; i < dustAmount; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center + new Vector2(-16,0).RotatedBy(Projectile.rotation) + new Vector2(-4, -4), 1, 1, DustID.Torch);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.15f, 0.55f) + MathHelper.Clamp((Projectile.velocity.Length() / 12f), 0.1f, 1.6f);
                //d.scale *= 0.9f;
                d.velocity += Projectile.velocity.RotatedBy(MathHelper.Pi)/12f * Main.rand.NextFloat(0.01f, 1.5f);
            }

            Projectile.netUpdate = true;

            //Main.chatMonitor.Clear();
            //Main.NewText(target);
            //Main.NewText(timeSinceSightCutOff);
        }


        // Checks if npc is closer than current targetNPC. If so, adjust targetNPC and closestTargetDistance.
        public int timeSinceSightCutOff = 0;
        private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC)
        {
            Player owner = Main.player[Projectile.owner];

            if (npc.CanBeChasedBy(this))
            {
                float distanceToTargetNPC = Vector2.Distance(Projectile.Center, npc.Center);
                // Is this enemy closer than others? Is it in line of sight?
                if (distanceToTargetNPC < closestTargetDistance)
                {
                    closestTargetDistance = distanceToTargetNPC; // Set a new closest distance value
                    targetNPC = npc;
                    if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height)) 
                    {
                        timeSinceSightCutOff = 0;
                    }
                }
                // if 
                if (!Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && !Collision.CanHit(owner.position, owner.width, owner.height, npc.position, npc.width, npc.height))
                {
                    timeSinceSightCutOff++;
                }
                if (owner.Center.Distance(npc.Center) > startAttackRange || timeSinceSightCutOff >= 30/* || !Collision.CanHit(owner.position, owner.width, owner.height, npc.position, npc.width, npc.height)*/)
                {
                    targetNPC = null;
                }
            }
        }

        private bool CheckAlive(Player owner)
        {

            if (!owner.active || owner.dead)
            {
                owner.ClearBuff(ModContent.BuffType<FighterJetBuff>());
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<FighterJetBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }
    }
}