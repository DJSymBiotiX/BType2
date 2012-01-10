using Microsoft.Xna.Framework;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    [Action("create")]
    [ActionParam(0, "id", typeof(string))]
    [ActionParam(1, "x", typeof(int))]
    [ActionParam(2, "y", typeof(int))]
    public sealed class CreateAction : Action
    {
        private string target;
        private string templateid;
        private Vector2 position;
        private static int Counter;

        public CreateAction(string target, string id, int x,  int y)
        {
            position = new Vector2(x,y);

            this.target = target;
            templateid = id;
        }

        public CreateAction(string target, string templateid, Vector2 position, ActionQueue actions)
        {
            this.target = target;
            this.templateid = templateid;
            this.position = position;
            this.actions = actions;
        }

        public override void Update(GameTime gameTime)
        {
            var targetName = templateid + "|" + Counter++;

            var enemy = CreateEnemy(targetName);

            var createTarget = ScriptingEngine.Item(target) as DrawableGameObject;
            if (createTarget != null) createTarget.Add(enemy);


            actions = new ActionQueue();
            foreach (var child in ScriptProvider.Templates[templateid].Elements())
            {
                actions.Add(ScriptProvider.ProcessAction(child, targetName));
            }

            ScriptingEngine.Add(targetName, enemy, actions);

            Finished = true;
        }


        public Sprite CreateEnemy(string name)
        {
            var enemy = new Sprite(position,
                    new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { 0.3f, 0.3f })) { Name = name };
            enemy.Scale = RNG.Next(50, 100)/100.0f;
            enemy.Rotation = RNG.Next(0, 500)/100.0f;

            return enemy;
        }
    }
}