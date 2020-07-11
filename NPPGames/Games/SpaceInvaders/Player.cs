using System;
using System.Collections.Generic;
using NPPGames.Core;
using NPPGames.Core.Audio;
using NPPGames.Core.Primitives;

namespace NPPGames.Games.SpaceInvaders
{
    public class Player : Sprite, ICollider, ITrajectory, IShooter
    {
        const string happy = @"    /\    
 ^ /~~\ ^ 
|--------|
|--------|";
        const string sad = @" *^ - - * 
*  -   - *
* -  ^   *
 *   ~  * ";
        public Player(GameData gameData, Scene scene)
            : base(gameData, scene, 10, 4)
        {
            base.SetData(happy);
            CollisionBehavior = CollisionBehavior.RunDestroySequence;
            DeathPlayer = new AudioTrackPlayer();
            DeathPlayer.AddTrack(new AudioTrack(NPPGames.Properties.Resources.SIPD));
            CollidesWithTypes = new List<string>() { SpriteTypes.ALIEN, SpriteTypes.ALIEN_BULLET };
        }
        private AudioTrackPlayer DeathPlayer { get; set; }

        private Bullet CurrentBullet { get; set; }

        public override void ResumeGame()
        {
            base.ResumeGame();
            base.SetData(happy);
        }

        #region ICollider members

        public void RunDestroySequence()
        {
            // we don't actually want to delete the player
            DeathPlayer.Play();
            Scene.Playerboard.RemoveLife();
            base.SetData(sad);
            ((SIGameData)GameData).PlayerDeath = true;
        }

        public void OnCollision(Sprite other)
        {
        }

        // disable the collided property because the player continues until the game detects that the player had died
        public bool HasCollided { get { return false; } set { } }

        public CollisionBehavior CollisionBehavior { get; set; }
        public IEnumerable<string> CollidesWithTypes { get; set; }

        #endregion

        #region ITrajectory members

        public int Frequency => 1;

        public Direction Direction { get; set; }

        public EdgeScreenHandling EdgeOfScreenCondition => EdgeScreenHandling.Stop;

        public TrajectoryRendererData TrajectoryRendererData { get; set; }

        #endregion

        #region IShooter members

        public void FireAtWill(TrajectoryController bulletController, ICollisionController collisionController)
        {
            if (null == CurrentBullet || CurrentBullet.MarkDelete)
            {
                CurrentBullet = new Bullet(GameData, Scene, 1, Direction.Up, NPPGames.Properties.Resources.SIPB)
                {
                    Direction = Direction.Up,
                    EdgeOfScreenCondition = EdgeScreenHandling.Disappear,
                    LayerOrder = int.MaxValue,
                    TrajectoryController = bulletController,
                    CollisionController = collisionController,
                    Type = SpriteTypes.PLAYER_BULLET,
                    CollidesWithTypes = new List<string>() { SpriteTypes.ALIEN, SpriteTypes.ALIEN_BULLET }
                };
                CurrentBullet.RendererData.StepY = 1;
                CurrentBullet.Bounds.Position.X = Bounds.Position.X + 4;
                CurrentBullet.Bounds.Position.Y = Bounds.Position.Y - 1;// move the missile down by one since the game will move it up on the first iteration of drawing
                CurrentBullet.Fire();
                Scene.AddSprite(CurrentBullet);
            }
        }

        #endregion

    }
}
