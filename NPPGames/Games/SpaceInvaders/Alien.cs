using System.Collections.Generic;
using NPPGames.Core;
using NPPGames.Core.Audio;
using NPPGames.Core.Primitives;

namespace NPPGames.Games.SpaceInvaders
{
    public class Alien : Sprite, ITrajectory, ICollider, IShooter
    {
        const string data = @"COME ON!!!!!";
        public Alien(GameData gameData, Scene scene, int frequency, Direction direction, Size size, int worth)
            : base(gameData, scene, size.Width, size.Height)
        {
            this.Worth = worth;
            Frequency = frequency;
            Direction = direction;
            base.SetData(data);
            DataIndex = 1; // set data index to 1 so it will flip back to 0 on first draw
            CollisionBehavior = CollisionBehavior.RunDestroySequence;
            DeathPlayer = new AudioTrackPlayer();
            DeathPlayer.AddTrack(new AudioTrack(NPPGames.Properties.Resources.SIAD));
            CollidesWithTypes = new List<string>() { SpriteTypes.PLAYER, SpriteTypes.PLAYER_BULLET };
        }

        public int Worth { get; set; }
        private Bullet AlienBullet { get; set; }
        private AudioTrackPlayer DeathPlayer { get; set; }

        public override bool MarkDelete
        {
            get
            {
                return _markDelete;
            }
            set
            {
                if(_markDelete != value)
                {
                    if(value)
                    {
                        Scene.Playerboard.AddPoints(this.Worth);
                    }
                    _markDelete = value;
                }
            }
        }
        private bool _markDelete;

        private int DataIndex { get; set; }

        public List<char> Data2 { get; set; }
        public void SetData2(string data)
        {
            Data2 = new List<char>();
            base.SetData(Data2, data);
        }

        public int Frequency { get; set; }

        public Direction Direction { get; set; }

        public EdgeScreenHandling EdgeOfScreenCondition { get; set; }
        public TrajectoryRendererData TrajectoryRendererData { get; set; }

        public override void Update()
        {
            base.Update();
            ++DataIndex;
            if (DataIndex > 1)
                DataIndex = 0;
        }
        public override char GetCharAt(int index)
        {
            if(DataIndex == 0)
            {
                if (index < Data.Count)
                {
                    return Data[index];
                }
                else
                {

                }
            }
            else
            {
                if (index < Data2.Count)
                {
                    return Data2[index];
                }
                else
                {

                }
            }
            return ' ';
        }

        public void FireAtWill(TrajectoryController bulletController, ICollisionController collisionController)
        {
            var positionabsolute = this.Bounds.Position + this.Bounds.OffsetPosition;
            positionabsolute.X += 3;
            positionabsolute.Y += 3;

            var bullet = new Bullet(GameData, Scene, 1, Direction.Up, NPPGames.Properties.Resources.SIAB)
            {
                EdgeOfScreenCondition = EdgeScreenHandling.Disappear,
                LayerOrder = int.MaxValue,
                TrajectoryController = bulletController,
                CollisionController = collisionController,
                Frequency = 2,
                Direction = Direction.Down,
                Type = SpriteTypes.ALIEN_BULLET,
                CollidesWithTypes = new List<string>() { SpriteTypes.PLAYER, SpriteTypes.PLAYER_BULLET }
            };
            bullet.SetData("!");
            bullet.RendererData.StepY = 1;
            bullet.Bounds.Position = positionabsolute;
            bullet.Fire();
            Scene.AddSprite(bullet);
        }

        #region ICollider members

        public void RunDestroySequence()
        {
            this.MarkDelete = true;
            DeathPlayer.Play();
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
