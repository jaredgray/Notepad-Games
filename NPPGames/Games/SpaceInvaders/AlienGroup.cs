using NPPGames.Core;
using NPPGames.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPPGames.Games.SpaceInvaders
{
    public class AlienGroup : SpriteGroup<Alien>, ITrajectory, ICollider
    {
        const string data = @"COME ON!!!!!";
        public AlienGroup(GameData gamedata, Scene scene, int frequency, Direction direction)
            : base(gamedata, scene)
        {
            Frequency = frequency;
            Direction = direction;
            EdgeOfScreenCondition = EdgeScreenHandling.ReverseDirection;
            base.SetData(data);
            CollisionBehavior = CollisionBehavior.None;
            CollidesWithTypes = new List<string>();
        }

        public int Frequency { get; set; }

        public Direction Direction { get; set; }

        public EdgeScreenHandling EdgeOfScreenCondition { get; set; }
        public TrajectoryRendererData TrajectoryRendererData { get; set; }

        public override void Update()
        {
            base.Update();
            if(this.Frequency == this.TrajectoryRendererData.IterationsSinceLastMovement)
            {
                foreach (var sprite in this)
                {
                    sprite.Update();
                }
                base.RebuildData();
            }
            RemoveDeleted();
        }

        private void RemoveDeleted()
        {
            var victimindexes = this.Where(x => x.MarkDelete).Select(x => this.IndexOf(x)).OrderByDescending(i => i);
            foreach (var victimIndex in victimindexes)
            {
                this.RemoveAt(victimIndex);
            }
            if (this.Count == 0)
                this.MarkDelete = true;
        }


        public bool HasCollided { get; set; }
        public CollisionBehavior CollisionBehavior { get; set; }
        public IEnumerable<string> CollidesWithTypes { get; set; }

        public void RunDestroySequence()
        {
            throw new NotImplementedException("An AlienGroup cannot run a DestroySequence");
        }

        public void OnCollision(Sprite other)
        {
        }
    }
}
