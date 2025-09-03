using CalamityVanilla.Content.Tiles.Furniture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Items.Placeable.Furniture
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
}
