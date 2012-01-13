using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Actions
{

    [Action("repeat")]
    [ActionParam(0, "count", typeof(int))]
    public sealed class RepeatAction : Action
    {
        private int Iterations { get; set; }
        private int CurrentIterations { get; set; }

        public RepeatAction(string target, int n)
        {
            if(n==0) n=-1;
            Iterations = n;
            CurrentIterations = n;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Actions.Queue.Count == 0)
            {
                if (CurrentIterations == 1)
                {
                    Finished = true;
                    CurrentIterations = Iterations;
                }
                else
                {
                    CurrentIterations--;
                    Reset();
                }
            }
        }
    }
}