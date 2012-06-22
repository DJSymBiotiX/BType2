using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Drawing.Sprites;
using Rollout.Drawing.Particles;
using Rollout.Scripting;
using Rollout.Scripting.Actions;

namespace B_Type_2_Dev
{
    public class Player : Sprite
    {
        public Dictionary<string, PlayerGun> Guns { get; set; }

        public Player()
        {
            Primary = true;

            Position = new Vector2(500,600);
            AddAnimation("main", Animation.Load("player"));
            Name = "player";
            //Shape = new Rectangle(0, 0, 64, 64);
            Shape = new Circle(0, 0, 8) {OffsetX = 24, OffsetY = 15};

            Guns = new Dictionary<string, PlayerGun>();

            var lifeforce = new Sprite(Vector2.Zero);
            lifeforce.AddAnimation("main", Animation.Load("lifeforce"));
            lifeforce.Name = "lifeforce";
            lifeforce.OffsetX = 24;
            lifeforce.OffsetY = 15;
            Add(lifeforce);

            var leftGun = new PlayerGun();
            leftGun.Position = new Vector2(8, 28);
            Add(leftGun);
            Guns.Add("left", leftGun);

            var rightGun = new PlayerGun();
            leftGun.Position = new Vector2(48, 28);
            Add(rightGun);
            Guns.Add("right", rightGun);

        }

    }

    public class PlayerGun : ParticlePool<PlayerBullet>, IFireable
    {
        public PlayerGun() : base()
        {
        }

        public void Fire()
        {
            var bullet = GetParticle();
            bullet.Reset();
            bullet.Position = new Vector2(X, Y);
            bullet.TimeToLive = 2;
            bullet.Enabled = true;
        }
    }

    public sealed class PlayerBullet : Particle
    {
        public PlayerBullet()
        {
            Name = "PlayerBullet_" + GetHashCode().ToString(CultureInfo.InvariantCulture);

            AddAnimation("main", Animation.Load("bullet"));
            Color = Color.LightCyan;
            Shape = new Circle(0, 0, 8);

            ScriptingEngine.Add(Name, this);    
            CollisionEngine.Add(this);
        }

        public new void Reset()
        {
            base.Reset();
            var action = new MoveToAction(Name, 0, "0-2000", 10f, "0");
            ScriptingEngine.AddAction(Name, action);
        }
    }
}