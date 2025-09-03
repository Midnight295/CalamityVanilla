using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Common
{
    internal class AmmoTypes : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            switch (entity.type)
            {
                /*case ItemID.PoisonedKnife:
                    entity.ammo = ItemID.ThrowingKnife;
                    entity.shoot = ProjectileID.PoisonedKnife;
                    break;
                case ItemID.ThrowingKnife:
                    entity.ammo = ItemID.ThrowingKnife;
                    entity.shoot = ProjectileID.ThrowingKnife;
                    break;*/
                case ItemID.Grenade:
                    entity.ammo = ItemID.Grenade;
                    entity.notAmmo = true;
                    break;
                case ItemID.BouncyGrenade:
                    entity.ammo = ItemID.Grenade;
                    entity.notAmmo = true;
                    break;
                case ItemID.StickyGrenade:
                    entity.ammo = ItemID.Grenade;
                    entity.notAmmo = true;
                    break;
                case ItemID.Beenade:
                    entity.ammo = ItemID.Grenade;
                    entity.notAmmo = true;
                    break;
            }
        }
    }
}