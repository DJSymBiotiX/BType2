﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rollout.Core;

namespace Rollout.Collision
{
    public class QuadTreeCollisionEngine : ICollisionEngine
    {
        private QuadTree quadTree;

        public QuadTreeCollisionEngine()
        {
            quadTree = new QuadTree(0, 0, G.Game.GraphicsDevice.Viewport.Width, G.Game.GraphicsDevice.Viewport.Height);

            quadTree.Split();
            quadTree.Split();
            quadTree.Split();
        }

        public void Add(ICollidable obj)
        {
            quadTree.Add(obj);
        }

        public void ProcessCollisions()
        {
            PairList<ICollidable> collisions = quadTree.GetCollisions();

            for (int i = 0; i < collisions.Count; i++)
            {
                Set<ICollidable> collision = collisions.Get(i);
                ICollidable a = collision.Get(0);
                ICollidable b = collision.Get(1);

                a.OnCollision(a, b);
                b.OnCollision(b, a);

            }
        }
    }
}
