﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class ScriptTest : Screen
    {
        private PlayerInput input;
        private Sprite player { get; set; }

        public override void Initialize()
        {
            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Left", Keys.Left);
            input.BindAction("Right", Keys.Right);
            input.BindAction("Up", Keys.Up);
            input.BindAction("Down", Keys.Down);

            player = new Sprite(new Vector2(500, 200)) { Name = "TheBiggest"};
            player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
            scriptingEngine.Add(player);
            Add(player);

            for (var i = 0; i < 2; i++)
            {
                var enemy = new Sprite(new Vector2(300 + 100 * i, 300), new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5)) { Name = "BigWilly" + i };
                scriptingEngine.Add(enemy);

                IAction moveloop = new RepeatAction(-1);

                if (i % 2 == 0)
                {
                    moveloop.AddAction(new MoveAction(enemy.Name, new Vector2(200, 200), Time.ms(100), scriptingEngine), true);
                    moveloop.AddAction(new MoveAction(enemy.Name, new Vector2(-200, -200), Time.ms(100), scriptingEngine), true);
                }
                else
                {
                    moveloop.AddAction(new FollowAction(enemy.Name, player.Name, .5f, scriptingEngine));
                }

                enemy.Actions.Add(moveloop);
                Add(enemy);
            }            

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.IsHeld("Left"))
                player.X -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.X += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Y -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Y += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin(Transition.Transform());
            base.Draw(gameTime);
            G.SpriteBatch.End();      
        }
    }
}
