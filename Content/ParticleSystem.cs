using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace ParticleEngine
{
    public static class ParticleSystem
    {
        private static int nextID;

        internal static readonly IList<Particle> particleTypes = new List<Particle>();

        public static IList<Particle> particle = new List<Particle>();

        internal static int ReserveParticleID() => nextID++;

        public static Particle GetParticle(int type) => (type == -1) ? null : particleTypes[type];

        internal static void Unload()
        {
            particleTypes.Clear();
            nextID = 0;
        }

        public static void UpdateParticles()
        {
            if (Main.dedServ || Main.gamePaused || Main.netMode == NetmodeID.Server)
                return;

            foreach (Particle item in particle.ToList())
            {
                item.position += item.velocity;
                item.Update();
                if (!item.Active)
                    particle.Remove(item);

            }
        }

        public static void DrawParticles(SpriteBatch spriteBatch)
        {
            if (Main.dedServ || Main.gameMenu || Main.netMode == NetmodeID.Server)
                return;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Rectangle value = new Rectangle((int)Main.screenPosition.X - Main.screenWidth, (int)Main.screenPosition.Y - Main.screenHeight, Main.screenWidth * 2, Main.screenHeight * 2);

            foreach (Particle particle in particle.Where((Particle p) => p.shader == null))
            {
                if (new Rectangle((int)particle.position.X - 3, (int)particle.position.Y - 3, 6, 6).Intersects(value))
                {
                    particle.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            foreach (Particle particle2 in particle.Where((Particle p) => p.shader != null))
            {
                if (new Rectangle((int)particle2.position.X - 3, (int)particle2.position.Y - 3, 6, 6).Intersects(value))
                {
                    particle2.shader.Apply(null);
                    particle2.Draw(spriteBatch);
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                }
            }

            spriteBatch.End();
        }
    }
}