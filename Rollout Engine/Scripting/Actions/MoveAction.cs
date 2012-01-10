using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing.Particle;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    [Action("move")]
    [ActionParam(0, "x", typeof(int))]
    [ActionParam(1, "y", typeof(int))]
    [ActionParam(2, "speed", typeof(double))]
    [ActionParam(3, "duration", typeof(int))]
    public sealed class MoveAction : Action
    {
        const double PixelsInAMeter = 100;

        private Vector2 TargetDelta;
        private Vector2 TotalDelta;
        private Vector2 DeltaRate;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private string targetName;
        private ITransformable target;

        private ITransformable Target
        {
            get { return target ?? (target = ScriptingEngine.Item(targetName) as ITransformable); }
        }

        public MoveAction(String targetName, int x, int y, double speed, int duration)
        {
            this.targetName = targetName;

            if (speed > 0)
            {    
                double distance = Math.Sqrt(x * x + y * y);
                Duration = Time.ms((int)(distance / speed * 1000 / PixelsInAMeter));
            }
            else
            {
                Duration = Time.ms(duration);   
            }
            
            TargetDelta = new Vector2(x,y);

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Reset();
        }

        public MoveAction(String targetName, Vector2 delta, double speed)
        {
            this.targetName = targetName;

            double distance = Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            Duration = Time.ms((int)(distance / speed * 1000 / PixelsInAMeter));
            TargetDelta = delta;

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Reset();
        }

        public MoveAction(String targetName, Vector2 delta, TimeSpan duration)
        {
            this.targetName = targetName;

            Duration = duration;
            TargetDelta = delta;

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            ElapsedTime = new TimeSpan();
            TotalDelta = new Vector2(0f, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime < Duration)
            {

                float dX = DeltaRate.X * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                float dY = DeltaRate.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds);

                TotalDelta.X += dX;
                TotalDelta.Y += dY;

                Target.X += dX;
                Target.Y += dY;

            }
            else
            {
                
                Target.X += TargetDelta.X - TotalDelta.X;
                Target.Y += TargetDelta.Y - TotalDelta.Y;

                Finished = true;
            }

        }
    }
}