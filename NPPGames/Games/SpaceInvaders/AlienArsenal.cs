using NPPGames.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPPGames.Games.SpaceInvaders
{
    public class AlienArsenal : Sprite
    {
        public AlienArsenal(GameData gameData, Scene scene, TrajectoryController bulletController, ICollisionController collisionController)
            : base(gameData, scene, 0, 0)
        {
            this.AlienGroups = new List<AlienGroup>();
            BulletController = bulletController;
            CollisionController = collisionController;
        }

        private TrajectoryController BulletController { get; set; }
        private List<AlienGroup> AlienGroups { get; set; }
        private DateTime NextFiring { get; set; }
        public bool IsStopped { get; set; }


        private void ResetNextFiring()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            NextFiring = DateTime.Now.AddMilliseconds(rnd.Next(600, 3000));
        }

        public void AddAlienGroup(AlienGroup group)
        {
            AlienGroups.Add(group);
        }

        public override void Update()
        {
            base.Update();
            if(DateTime.Now > NextFiring)
            {
                FireAtRandom();
            }
        }

        public void Clear()
        {
            this.AlienGroups = new List<AlienGroup>();
        }
        
        public void Stop()
        {
            IsStopped = true;
        }

        private void FireAtRandom()
        {
            if (IsStopped)
                return;

            this.AlienGroups.RemoveAll(x => x.MarkDelete);
            Random rnd = new Random(DateTime.Now.Millisecond);


            var sortedgroups = this.AlienGroups.OrderByDescending(x => x.Bounds.Position.Y);
            Dictionary<int, Tuple<AlienGroup, Alien>> candidates = new Dictionary<int, Tuple<AlienGroup, Alien>>();

            StringBuilder sb = new StringBuilder();
            foreach (var group in sortedgroups)
            {
                foreach (var alien in group)
                {
                    sb.Append($"{alien.Bounds.Position.X},");
                }
                sb.AppendLine();
            }

            var results = sb.ToString();

            foreach (var group in sortedgroups)
            {
                foreach (var alien in group)
                {
                    if (alien.MarkDelete)
                        continue;
                    if (!candidates.ContainsKey(alien.Bounds.Position.X))
                        candidates.Add(alien.Bounds.Position.X, new Tuple<AlienGroup, Alien>(group, alien));
                }
            }

            if (candidates.Count < 1)
                return;

            var index = rnd.Next(0, candidates.Count - 1);
            var shooter = candidates.ElementAt(index);
            shooter.Value.Item2.FireAtWill(BulletController, base.CollisionController);

            ResetNextFiring();
        }
    }
}
