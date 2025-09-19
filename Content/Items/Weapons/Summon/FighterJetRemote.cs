using CalamityVanilla.Common.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Weapons.Summon
{
    public class FighterJetRemote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.damage = 20;
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

            Item.UseSound = SoundID.Item1;
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
        private Vector2 push = Vector2.Zero;
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
            DrawOriginOffsetY = 6;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Projectile.rotation = (Projectile.velocity).ToRotation();

            if (!CheckAlive(owner)) return;
            /*-
            if (Projectile.Center.Distance(owner.Center) > 20)
            {
                
                float dist = Projectile.Center.Distance(owner.Center) / 50f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(owner.Center) * dist, 0.05f) * 1f;
                Projectile.velocity = CVUtils.LengthClamp(Projectile.velocity, 20f, 2f);
                
            }
            */

            PlayerStats stats = owner.GetModPlayer<PlayerStats>();
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 1)
            {
                Vector2 targetPos = new Vector2(200, 0).RotatedBy((stats.TimeInWorld * 0.05f) + (MathHelper.TwoPi / (owner.ownedProjectileCounts[Type]) * Projectile.minionPos));

                targetPos.Y *= MathF.Sin((stats.TimeInWorld * 0.05f) + (Projectile.identity * 1.5f) + (stats.TimeInWorld/100f)) * 0.75f;
                targetPos.X *= 0.9f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(targetPos + owner.Center) * 8f, 0.1f) + owner.velocity/12f;
                //Projectile.position = targetPos;
            }

            int val = 1;

            if (Projectile.velocity.Length() > 8f)
            {
                val = 3;
            }

            for (int i = 0; i < val; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.Center + new Vector2(-14,-4).RotatedBy(Projectile.rotation), 1, 1, DustID.Torch);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(0.25f, 0.75f) + (Projectile.velocity.Length() / 12f);
                //d.scale *= 0.9f;
                d.velocity += Projectile.velocity.RotatedBy(MathHelper.Pi)/10f * Main.rand.NextFloat(0.5f, 1.5f);
            }

            Projectile.netUpdate = true;

            int startAttackRange = 700;
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(startAttackRange, ref attackTarget, false);
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