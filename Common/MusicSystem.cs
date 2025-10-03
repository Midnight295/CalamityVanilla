using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityVanilla.Common
{
    public class MusicSystem : ModSystem
    {
        public override void SetStaticDefaults()
        {
            MusicID.Sets.SkipsVolumeRemap[MusicLoader.GetMusicSlot(Mod, "Assets/Music/HiveMind")] = true;
            MusicID.Sets.SkipsVolumeRemap[MusicLoader.GetMusicSlot(Mod, "Assets/Music/Cryogen")] = true;
        }
    }
}
