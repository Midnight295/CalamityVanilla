﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityVanilla.Content.Tiles.Furniture
{
    public class IceSculptureAngel : ModItem
    {
        public virtual int Style => 0;
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<IceSculptures>(), Style);
        }
    }
    public class IceSculptureArmor : IceSculptureAngel
    {
        public override int Style => 1;
    }
    public class IceSculptureHelix : IceSculptureAngel
    {
        public override int Style => 2;
    }
    public class IceSculptureHorse : IceSculptureAngel
    {
        public override int Style => 3;
    }
    public class IceSculptures : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleWrapLimit = 4;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16, 16];
            TileObjectData.newTile.Origin = new Point16(2, 5);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(TileObjectData.newTile.StyleWrapLimit);
            TileObjectData.addTile(Type);
            DustType = DustID.Ice;
            HitSound = SoundID.Item27;
            AddMapEntry(new Color(100, 191, 255), this.GetLocalization("MapEntry"));
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            int item = 0;
            switch (Main.tile[i, j].TileFrameX / 72)
            {
                case 0:
                    item = ModContent.ItemType<IceSculptureAngel>();
                    break;
                case 1:
                    item = ModContent.ItemType<IceSculptureArmor>();
                    break;
                case 2:
                    item = ModContent.ItemType<IceSculptureHelix>();
                    break;
                case 3:
                    item = ModContent.ItemType<IceSculptureHorse>();
                    break;
            }
            yield return new Item(item);
        }
    }
}
