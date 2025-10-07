using CalamityVanilla.Common.ItemDropRules.DropConditions;
using CalamityVanilla.Content.Items.Material;
using CalamityVanilla.Content.Items.Weapons.Magic;
using CalamityVanilla.Content.Items.Weapons.Ranged;
using CalamityVanilla.Content.Items.Weapons.Ranged.Throwing;
using CalamityVanilla.Content.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Diagnostics.Contracts;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityVanilla.Common
{
    public class CVGlobalItem : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            switch (item.type)
            {
                case ItemID.WallOfFleshBossBag:
                    itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<FleshLauncher>(), ModContent.ItemType<PyrobatStaff>(), ModContent.ItemType<FighterJetRemote>()));
                    break;
            }
        }
        public static int Style = 1;
        public static Color ENNWAYColor = new(52, 57 , 92);
        public static int lerp;

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {   
            //Replaces instances of ENNWAY! in tooltip lines with custom drawing to have fancy color swapping.
            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
            {
                if (line.Text.Contains("ENNWAY!"))
                {
                    // 52, 57, 92
                    // 17, 81, 119
                    // 72, 139, 154

                    /*if (Style == 1)
                    {
                        ENNWAYColor.R -= 2;
                        ENNWAYColor.B += 2;
                        ENNWAYColor.G += 2;
                        if (ENNWAYColor.B >= 110 && ENNWAYColor.R <= 17 && ENNWAYColor.G >= 81)
                        {
                            Style = 2;
                        }

                    }
                    if (Style == 2)
                    {
                        ENNWAYColor.R += 2;
                        ENNWAYColor.B += 2;
                        ENNWAYColor.G += 2;

                        if (ENNWAYColor.R >= 72 && ENNWAYColor.G >= 139 && ENNWAYColor.B >= 154)
                        {
                            Style = 3;
                        }

                    }
                    if (Style == 3)
                    {
                        ENNWAYColor.R -= 2;
                        ENNWAYColor.B -= 2;
                        ENNWAYColor.G -= 2;
                        if (ENNWAYColor.R <= 17 && ENNWAYColor.G <= 81 && ENNWAYColor.B <= 119)
                        {
                            Style = 4;
                        }

                    }
                    if (Style == 4)
                    {
                        ENNWAYColor.R += 2;
                        ENNWAYColor.B -= 2;
                        ENNWAYColor.G -= 2;
                        if (ENNWAYColor.R >= 52 && ENNWAYColor.G <= 57 && ENNWAYColor.B <= 92)
                        {
                            Style = 1;
                            //Lerp = 0;
                        }

                    }*/
                    //ENNWAYColor = Color.Lerp(new(52, 57, 92), new(17, 81, 119), Main.GlobalTimeWrappedHourly * 3);
                    Vector3 color = Vector3.Lerp(new Vector3(52, 57, 92), new Vector3(17,81, 119), ++lerp * 0.07f);
                    ENNWAYColor = new(color.X, color.Y, color.Z);
                    Main.NewText(lerp);
                    Main.NewText(color);

                    string text = line.Text.Replace("ENNWAY!", "");
                    Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One);
                    
                    Utils.DrawBorderString(Main.spriteBatch, text, new Vector2(line.X, line.Y), Color.White, 1);
                    Utils.DrawBorderString(Main.spriteBatch, "ENNWAY!", new Vector2(line.X +stringSize.X, line.Y), ENNWAYColor, 1);
                    return false;
                }
            }
            return true;
        }
    }
}
