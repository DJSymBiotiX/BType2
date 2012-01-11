using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Screens;

namespace Rollout.Collision
{
    public static class CollisionEngine
    {
        private static Dictionary<string, ICollisionEngine> Engines = new Dictionary<string, ICollisionEngine>();
        public static ICollisionEngine Engine
        {
            get
            {
                if (!Engines.ContainsKey(ScreenContext.ContextKey))
                    Engines.Add(ScreenContext.ContextKey, new QuadTreeCollisionEngine());
                return Engines[ScreenContext.ContextKey];
            }
        }

        public static bool Debug { get; set; }

        public static void Add(ICollidable obj)
        {
            Engine.Add(obj);
        }

        public static void Update(GameTime gameTime)
        {
            Engine.Update(gameTime);
        }


        public static void Register<TSender, TObject>(Action<TSender, TObject> eventHandler)
            where TSender : ICollidable
            where TObject : ICollidable
        {
            Engine.Register(eventHandler);
        }


        public static void Draw(GameTime gameTime)
        {
            Engine.Draw(gameTime);
        }
    }
}