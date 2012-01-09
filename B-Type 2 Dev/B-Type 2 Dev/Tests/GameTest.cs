﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Collision;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Rectangle;

namespace B_Type_2_Dev
{

    public class Enemy
    {
        public Sprite Sprite { get; set; }

        public Enemy(Screen numberOne)
        {


            Sprite = new Sprite(new Vector2(0, 0), Animation.Load("player")) { Name = "enemy" };
            Sprite.Rotation = MathHelper.Pi;

            Sprite.Shape = new Rectangle(0,0,64,64);
            Sprite.OnCollision += (src, obj) => ((Sprite)src).Rotation += 0.1f;

            numberOne.Add(Sprite);
        }
    }

    public class Player
    {
        public Sprite Sprite { get; set; }
        public Dictionary<string, Gun> Guns { get; set; }

        public Player(Screen screen)
        {
            
            Sprite = new Sprite(new Vector2(200, 400), Animation.Load("player")) { Name = "player" };
            screen.Add(Sprite);            
            
            
            Guns = new Dictionary<string, Gun>();
            Guns.Add("left", new Gun(screen,"left", new Vector2(-21, 20), new Vector2(-114, -128)));
            Guns.Add("right", new Gun(screen,"right", new Vector2(57, 20), new Vector2(-114, -128)));

            foreach (var gun in Guns.Values)
            {
                Sprite.Add(gun.Sprite);       
            }
        }

    }

    public class Gun
    {
        public Sprite Sprite { get; set; }
        public ParticleEmitter Emitter { get; set; }

        public Gun(Screen screen, string name, Vector2 gunOffset, Vector2 emitterOffset)
        {
            Sprite = new Sprite(gunOffset, new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { 0.05f, 0.08f }, false)) { Name = name };

            Sprite.Screen = screen;

            Emitter = new ParticleEmitter(200, screen)
            {
                Name = name + "-emitter",
                OffsetX = emitterOffset.X,
                OffsetY = emitterOffset.Y
            };

            Sprite.Add(Emitter);
        }

        public void Fire()
        {
            Sprite.ReStart();
            Emitter.Fire2();   
        }

    }

    public class GameTest : Screen
    {
        private ICollisionEngine collisionEngine;

        private PlayerInput input;

        private Player player;
        private bool fireRight = true;
        private Limiter fireLimit;

        private double elapsedTime;

        private Enemy enemy;

        public override void Initialize()
        {
            // Must happen at the start
            base.Initialize();

            AnimationLoader.Test();

            enemy = new Enemy(this);

            player = new Player(this);
          
            input = new PlayerInput(PlayerIndex.One);
            input.BindAction("Quit", Keys.Q);
            input.BindAction("Left", Keys.Left);
            input.BindAction("Right",Keys.Right);
            input.BindAction("Up",Keys.Up);
            input.BindAction("Down",Keys.Down);
            input.BindAction("Fire",Keys.Space);

            fireLimit = new Limiter(.05f);

            collisionEngine = Screen.collisionEngine;
            collisionEngine.Add(player.Sprite);
            collisionEngine.Add(enemy.Sprite);
        }

        public override void Update(GameTime gameTime)
        {
            int speed = 500;
            fireLimit.Update(gameTime);

            if (input.IsPressed("Quit"))
                G.Game.Exit();

            if (input.IsHeld("Left"))
                player.Sprite.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.Sprite.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Sprite.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Sprite.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (input.IsPressed("Fire"))
            {
                elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
                FireGun();
            }

            if (input.IsReleased("Fire"))
            {
                elapsedTime = 0;
            }

            if (input.IsHeld("Fire"))
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (elapsedTime > 0.2f)
                {
                    FireGun();
                }
            }

            base.Update(gameTime);
        }

        private void FireGun()
        {
            if (fireLimit.Ready)
            {
                 if (fireRight)
                {
                    player.Guns["right"].Fire();
                }
                else
                {
                    player.Guns["left"].Fire();
                }
                fireRight = !fireRight;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin(Transition.Transform());
            base.Draw(gameTime);
            G.SpriteBatch.End();            
        }
    }
}
