using CalamityVanilla.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityVanilla.Content.Tiles.Furniture.MusicBoxes
{
    public class HiveMindMusicBox : BaseMusicBox.MusicBoxItem
    {
        public override void SetStaticDefaults()
        {   
            //TODO: Change the asset path to HiveMind music when the track for it exists.
            MusicLoader.AddMusicBox(
            Mod,
            MusicLoader.GetMusicSlot(Mod, "Assets/Music/HiveMind"),
            Type,
            TileType);
            
            base.SetStaticDefaults();
        }
        public override int TileType => ModContent.TileType<HiveMindMusicBoxTile>();
    }

    public class HiveMindMusicBoxTile : BaseMusicBox.MusicBoxTile { }
}
