﻿using Microsoft.Xna.Framework;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;

namespace Rollout.Drawing.Particles
{
    public interface IParticle : IGameObject
    {
        double Age { get; }
        double TimeToLive { get; }
        double ElapsedTime { get; }
        bool Enabled { get; set; }
    }

    public class Particle : Sprite, IParticle
    {
        public double Age { get; private set; }
        public double TimeToLive { get; set; }
        public double ElapsedTime { get; private set; }

        public Particle() 
        {
            Reset();
        }

        public void Reset()
        {
            Age = 0;
            ElapsedTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Age += gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
    }
}
