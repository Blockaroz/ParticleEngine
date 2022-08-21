using On.Terraria;
using Terraria;

namespace ParticleEngine
{
    public static class ParticleLoader
    {
        public static void Load()
        {
            On.Terraria.Main.DrawDust += DrawParticles;
            On.Terraria.Main.UpdateParticleSystems += UpdateParticles;
        }

        public static void Unload()
        {
            ParticleSystem.Unload();
        }

        private static void DrawParticles(On.Terraria.Main.orig_DrawDust orig, Terraria.Main self)
        {
            orig(self);
            ParticleSystem.DrawParticles(Terraria.Main.spriteBatch);
        }

        private static void UpdateParticles(On.Terraria.Main.orig_UpdateParticleSystems orig, Terraria.Main self)
        {
            orig(self);
            ParticleSystem.UpdateParticles();
        }
    }
}