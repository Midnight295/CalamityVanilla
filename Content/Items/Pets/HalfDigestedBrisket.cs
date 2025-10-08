using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Pets
{
    public class HalfDigestedBrisket : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToVanitypet(ModContent.ProjectileType<StomachBugPet>(), ModContent.BuffType<StomachBugBuff>());
            Item.master = true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            base.UseStyle(player, heldItemFrame);

            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }

    public class StomachBugBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<StomachBugPet>());
        }
    }

    public class StomachBugPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Type] = true;
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 54;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
            Projectile.netImportant = true;
            Projectile.timeLeft *= 5;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public Vector2 Nextposition;

        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            Nextposition = owner.Center;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!owner.active)
            {
                Projectile.active = false;
                return;
            }
            if (!owner.dead && owner.HasBuff(ModContent.BuffType<StomachBugBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            if (Projectile.Center.Distance(owner.Center) > 2000)
            {
                Projectile.Center = owner.Center;
            }

            if (Projectile.velocity.HasNaNs())
                Projectile.Center = owner.position;

            if (Projectile.Distance(Nextposition) <= 40)
            {
                Projectile.ai[0]++;
                Projectile.ai[0] += (Projectile.Distance(owner.Center) * 0.01f);
            }

            Main.NewText(Projectile.ai[0]);
            Main.NewText(Projectile.Distance(owner.Center) * 0.01f);
            if (Projectile.ai[0] >= 55)
            {
                Projectile.ai[0] = 0;
                Nextposition = 
                    owner.position +
                    (Main.rand.NextVector2Circular(75, 75) 
                    * ((Projectile.Distance(owner.Center) * 0.01f) + 1));
            }

            Vector2 position = Nextposition;

            Projectile.spriteDirection = Projectile.Center.X >= owner.Center.X? -1 : 1;

            Projectile.velocity = Projectile.DirectionTo(position) * MathHelper.Lerp(0, 5, Projectile.Distance(Nextposition) * 0.02f);
            Projectile.velocity = Projectile.velocity.LengthClamp(22);
            Projectile.rotation = Projectile.velocity.Length() / 30f * Projectile.direction;

            if (++Projectile.frameCounter == 3)
            {
                ++Projectile.frame;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
