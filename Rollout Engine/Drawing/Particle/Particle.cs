﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Scripting;

namespace Rollout.Drawing.Particle
{
    public interface IParticle
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

        public Particle(Vector2 startPosition, Animation animation = null, string animationName = "main") 
            : base(startPosition, animation, animationName)
        {
            Initialize();
        }

        public override void Initialize()
        {
            Age = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Age += gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }
    }
}
