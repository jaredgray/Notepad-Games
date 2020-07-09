using NPPGames.Core;
using NPPGames.Core.Audio;
using NPPGames.Core.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPPGames.Games.SpaceInvaders
{
    public class Bullet : Sprite, ITrajectory, ICollider
    {
        const string data = @"*";
        public Bullet(GameData gameData, Scene scene, int frequency, Direction direction, Stream fireSound)
            : base(gameData, scene, 1, 1)
        {
            Frequency = frequency;
            Direction = direction;
            base.SetData(data);
            CollisionBehavior = CollisionBehavior.Remove;
            Player = new AudioTrackPlayer();
            Player.AddTrack(new AudioTrack(fireSound));
        }

        public AudioTrackPlayer Player { get; set; }

        public int Frequency { get; set; }

        public Direction Direction { get; set; }

        public EdgeScreenHandling EdgeOfScreenCondition { get; set; }
        public TrajectoryRendererData TrajectoryRendererData { get; set; }

        public void Fire()
        {
            Player.Play();
        }

        #region ICollider members

        public void RunDestroySequence()
        {
            throw new NotImplementedException("A Bullet cannot run a DestroySequence");
        }

        public void OnCollision(Sprite other)
        {
        }

        public bool HasCollided { get; set; }
        public CollisionBehavior CollisionBehavior { get; set; }
        public IEnumerable<string> CollidesWithTypes { get; set; }

        #endregion
    }
}
