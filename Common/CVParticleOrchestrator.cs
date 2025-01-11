using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.NetModules;
using Terraria.Graphics.Renderers;
using Terraria.Net;

namespace CalamityVanilla.Common
{
    // this is just a class that takes some stuff from vanillas particle orchestrator to use its particles
    public class CVParticleOrchestrator
    {
        public static PrettySparkleParticle RequestPrettySparkleParticle()
        {
            return _poolPrettySparkle.RequestParticle();
        }

        private static PrettySparkleParticle GetNewPrettySparkleParticle() => new PrettySparkleParticle();
        private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new ParticlePool<PrettySparkleParticle>(200, new ParticlePool<PrettySparkleParticle>.ParticleInstantiator(GetNewPrettySparkleParticle));
    }
}
