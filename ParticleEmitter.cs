using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.VisualBasic.CompilerServices;
//using System.Numerics;

namespace Tetris
{
    public class Particle
    {
        public int name;
        public Vector2 position;
        public float rotation;
        public Vector2 direction;
        public float speed;
        public TimeSpan lifetime;
        public Texture2D texture;

        public Particle(int name, Vector2 position, Vector2 direction, float speed, TimeSpan lifetime, Texture2D texture)
        {
            this.name = name;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.lifetime = lifetime;
            this.texture = texture;

            this.rotation = 0;
        }
    }

    class ParticleEmitter
    {
        private Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        private Texture2D iCell;
        private Texture2D jCell;
        private Texture2D lCell;
        private Texture2D oCell;
        private Texture2D sCell;
        private Texture2D tCell;
        private Texture2D zCell;
        private Texture2D blankCell;
        private List<Texture2D> cellTextures;
        private MyRandom random = new MyRandom();

        private int m_sourceX;
        private int m_sourceY;
        private int m_particleSize;
        private int m_speed;
        private TimeSpan m_lifetime;
        public Vector2 Gravity { get; set; }
        public Vector2 cellSize { get; set; }

        public ParticleEmitter(ContentManager content, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime, Vector2 cell)
        {
            m_sourceX = sourceX;
            m_sourceY = sourceY;
            m_particleSize = size;
            m_speed = speed;
            m_lifetime = lifetime;

            cellSize = cell;

            cellTextures = new List<Texture2D>();

            iCell = content.Load<Texture2D>("iCell");
            jCell = content.Load<Texture2D>("jCell");
            lCell = content.Load<Texture2D>("lCell");
            oCell = content.Load<Texture2D>("oCell");
            sCell = content.Load<Texture2D>("sCell");
            tCell = content.Load<Texture2D>("tCell");
            zCell = content.Load<Texture2D>("zCell");
            blankCell = content.Load<Texture2D>("blankCell");

            cellTextures.Add(iCell);
            cellTextures.Add(jCell);
            cellTextures.Add(lCell);
            cellTextures.Add(oCell);
            cellTextures.Add(sCell);
            cellTextures.Add(tCell);
            cellTextures.Add(zCell);
            cellTextures.Add(blankCell);

            this.Gravity = new Vector2(0, 0.1f);
        }

        public int ParticleCount
        {
            get { return m_particles.Count; }
        }

        private TimeSpan m_accumulated = TimeSpan.Zero;

        /// <summary>
        /// Generates new particles, updates the state of existing ones and retires expired particles.
        /// </summary>
        /// 
        public void lineClear(GameTime gameTime, Vector2 position, float angle, int particleType, double dev = 0)
        {
            m_sourceX = (int)position.X;
            m_sourceY = (int)position.Y;


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Particle p = new Particle(
                        random.Next(),
                        new Vector2(m_sourceX + (cellSize.X / 3 * i), m_sourceY + (cellSize.Y / 3 * j)),
                        random.nextCircleVector(-1, dev),
                        (float)random.nextGaussian(m_speed, Math.Sqrt(m_speed)),
                        m_lifetime,
                        cellTextures[particleType]);

                    if (!m_particles.ContainsKey(p.name))
                    {
                        m_particles.Add(p.name, p);
                    }
                }
            }
        }

        public void shapeSet(GameTime gameTime, Vector2 position, float angle, int particleType, double dev = 0)
        {
            m_sourceX = (int)position.X;
            m_sourceY = (int)position.Y;


            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    Particle p = new Particle(
                        random.Next(),
                        new Vector2(m_sourceX + (cellSize.X / 1.5f * i), m_sourceY + (cellSize.Y / 1.5f * j)),
                        random.nextCircleVector(-1, dev),
                        (float)random.nextGaussian(m_speed, Math.Sqrt(m_speed)),
                        m_lifetime,
                        cellTextures[particleType]);

                    if (!m_particles.ContainsKey(p.name))
                    {
                        m_particles.Add(p.name, p);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= gameTime.ElapsedGameTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
                else
                {
                    //
                    // Only if we have enough elapsed time, and then move/rotate things
                    // based upon elapsed time, not just the fact that we have received an update.
                    if (gameTime.ElapsedGameTime.Milliseconds > 0)
                    {
                        //
                        // Update its position
                        p.position += (p.direction * (p.speed * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f)));

                        p.speed *= 0.999f;
                        //
                        // Have it rotate proportional to its speed
                        p.rotation += (p.speed * (gameTime.ElapsedGameTime.Milliseconds / 100000.0f));
                    }

                    //
                    // Apply some gravity
                    p.direction += this.Gravity;
                }
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {

            Rectangle r = new Rectangle(0, 0, m_particleSize, m_particleSize);
            foreach (Particle p in m_particles.Values)
            {
                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    p.texture,
                    r,
                    null,
                    Color.White,
                    p.rotation,
                    new Vector2(p.texture.Width / 2, p.texture.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }

    /// <summary>
    /// Expands upon some of the features the .NET Random class does:
    /// 
    /// *NextRange : Generate a random number within some range
    /// *NextGaussian : Generate a normally distributed random number
    /// 
    /// </summary>
    class MyRandom : Random
    {

        /// <summary>
        /// Generates a random number in the range or [Min,Max]
        /// </summary>
        public float nextRange(float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)this.NextDouble());
        }

        /// <summary>
        /// Generate a random vector about a unit circle
        /// </summary>
        public Vector2 nextCircleVector(float angle = -1, double stdDev = 0.1)
        {
            if (angle == -1)
                angle = (float)(this.NextDouble() * 2.0 * Math.PI);
            else
            {
                angle = (float)nextGaussian(angle, stdDev);
            }
            float x = (float)Math.Cos(angle + Math.PI / 2);
            float y = (float)Math.Sin(angle + Math.PI / 2);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Generate a normally distributed random number.  Derived from a Wiki reference on
        /// how to do this.
        /// </summary>
        public double nextGaussian(double mean, double stdDev)
        {
            if (this.usePrevious)
            {
                this.usePrevious = false;
                return mean + y2 * stdDev;
            }
            this.usePrevious = true;

            double x1 = 0.0;
            double x2 = 0.0;
            double y1 = 0.0;
            double z = 0.0;

            do
            {
                x1 = 2.0 * this.NextDouble() - 1.0;
                x2 = 2.0 * this.NextDouble() - 1.0;
                z = (x1 * x1) + (x2 * x2);
            }
            while (z >= 1.0);

            z = Math.Sqrt((-2.0 * Math.Log(z)) / z);
            y1 = x1 * z;
            y2 = x2 * z;

            return mean + y1 * stdDev;
        }

        /// <summary>
        /// Keep this around to optimize gaussian calculation performance.
        /// </summary>
        private double y2;
        private bool usePrevious { get; set; }
    }
}



