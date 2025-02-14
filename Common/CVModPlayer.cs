using CalamityVanilla.Content.Items.Weapons.Ranged;
using CalamityVanilla.Content.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Common
{
    public class CVModPlayer : ModPlayer
    {
        public override void Load()
        {
            ModContent.GetInstance<CalamityVanilla>().Logger.Info("Inject start");
            IL_Player.Update += EnableStealth;
        }
        public override void Unload()
        {
            ModContent.GetInstance<CalamityVanilla>().Logger.Info("NotInject start");
            IL_Player.Update -= EnableStealth;
        }
        private void EnableStealth(ILContext il)
        {
            ILCursor cursor = new(il);
            if (!cursor.TryGotoNext(MoveType.Before, i => i.MatchLdfld(typeof(Player), nameof(Player.manaSick))))
                return;

            ILLabel skipCustomBehavior = il.DefineLabel();
            ILLabel skipDefaultBehavior = il.DefineLabel();
            cursor.Index -= 1;
            cursor.MarkLabel(skipDefaultBehavior);
            cursor.Index -= 2;

            cursor.EmitDelegate((Player player) =>
            {
                return Content.Items.Weapons.Ranged.Crystaline.IsHoldingCrystaline(player);
            });
            cursor.Emit(OpCodes.Brfalse, skipCustomBehavior);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate((Player player) =>
            {
                Content.Items.Weapons.Ranged.Crystaline.UpdateCrystalineStealth(player);
            });
            cursor.Emit(OpCodes.Br, skipDefaultBehavior);
            cursor.MarkLabel(skipCustomBehavior);
            cursor.Emit(OpCodes.Ldarg_0);
        }

        public bool StealthEnabled = false;
        public override void ResetEffects()
        {
            StealthEnabled = false;
        }
        public int GothicToothRegenCounter = 0;
        public override void PostUpdateBuffs()
        {
            GothicToothRegenCounter++;
            if (GothicToothRegenCounter > 120)
            {
                if (!Player.moonLeech)
                {
                    int lifeRegen = 0;
                    foreach(Projectile tooth in Main.ActiveProjectiles)
                    {
                        if(tooth.type == ModContent.ProjectileType<GothicTooth>() && tooth.owner == Player.whoAmI && tooth.ai[0] == 1 && Main.npc[(int)tooth.ai[1]].type != NPCID.TargetDummy)
                        {
                            lifeRegen++;
                        }
                    }
                    if (lifeRegen == 0)
                        return;
                    if (lifeRegen > 30)
                        lifeRegen = 30;
                    Player.statLife += lifeRegen;
                    CombatText.NewText(Player.Hitbox, CombatText.HealLife, lifeRegen);
                    for(int i = 0; i < lifeRegen * 2; i++)
                    {
                        Vector2 rotation = Main.rand.NextVector2Circular(1,1);
                        Dust d = Dust.NewDustPerfect(Player.Center + rotation * 40, DustID.VampireHeal);
                        d.velocity = -rotation * 3 + Player.velocity;
                        d.scale *= 1.3f;
                        d.noGravity = true;
                    }
                }
                GothicToothRegenCounter = 0;
            }
        }
    }
}
