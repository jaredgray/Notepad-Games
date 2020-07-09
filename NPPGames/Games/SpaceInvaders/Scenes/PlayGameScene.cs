using System.Linq;
using System.Threading;
using NPPGames.Games.SpaceInvaders.Scenes;
using NPPGames.Core;
using NPPGames.Core.Audio;
using NPPGames.Core.Primitives;

namespace NPPGames.Games.SpaceInvaders
{
    public class PlayGameScene : SIScene
    {
        public PlayGameScene(IRenderer renderer, Logger logger, Size size, GameData gameData) : base(renderer, logger, size, gameData)
        {
        }


        public AudioLoop MainAudioPlayer { get; set; }

        public bool IsGameOver { get; set; }

        readonly string PlayerData = @"    /\    
 ^ /~~\ ^ 
|--------|
|--------|";
        readonly string AlienData30A = @"{@ @} 
 / \  "; // 1 row
        readonly string AlienData30B = @"{@ @} 
 \ /  "; // 1 row
        readonly string AlienData20A = @" '  ' 
\^/\^/"; // 2 rows
        readonly string AlienData20B = @" '  ' 
/^\/^\"; // 2 rows
        readonly string AlienData10A = @"/[..]\
 |~~| "; // 2 rows
        readonly string AlienData10B = @"/[..]\
 /~~\ "; // 2 rows
        public Player Player { get; set; }


        private TrajectoryController TrajectoryController { get; set; }
        private TrajectoryController AlienController { get; set; }
        private AlienArsenal AlienArsenal { get; set; }
        public CollisionController CollisionController { get; set; }

        public override void InitializeScene()
        {
            IsGameOver = false;
            ClearSprites();
            Logger.WriteLine($"Initializing AsciiShooter...");
            TrajectoryController = new TrajectoryController(Size, Logger);
            AlienController = new AlienTrajectoryController(Size, Logger);
            CollisionController = new CollisionController();
            AlienArsenal = new AlienArsenal(GameData, this, TrajectoryController, CollisionController);
            SetPlayerboard(new Playerboard("/\\", 3));
            SetupAudio();
            BuildCharacters();
        }

        private void SetupAudio()
        {
            MainAudioPlayer = new AudioLoop();
            MainAudioPlayer.AddTrack(new DelayedAudioTrack(NPPGames.Properties.Resources.SI01)
            {
                DelayType = DelayType.WaitEnd,
                WaitInMilliseconds = 600
            });
            MainAudioPlayer.AddTrack(new DelayedAudioTrack(NPPGames.Properties.Resources.SI02)
            {
                DelayType = DelayType.WaitEnd,
                WaitInMilliseconds = 600
            });
            MainAudioPlayer.AddTrack(new DelayedAudioTrack(NPPGames.Properties.Resources.SI03)
            {
                DelayType = DelayType.WaitEnd,
                WaitInMilliseconds = 600
            });
            MainAudioPlayer.AddTrack(new DelayedAudioTrack(NPPGames.Properties.Resources.SI04)
            {
                DelayType = DelayType.WaitEnd,
                WaitInMilliseconds = 600
            });
        }

        private void BuildCharacters()
        {
            //Player = new Sprite(14, 5)
            Player = new Player(GameData, this, 10, 4)
            {
                LayerOrder = 0,
                Type = SpriteTypes.PLAYER,
                TrajectoryController = TrajectoryController
            };
            Player.SetData(PlayerData);
            Player.Bounds.Position.X = 45;
            Player.Bounds.Position.Y = 36;
            Player.RendererData.StepX = 1;
            Player.RendererData.StepY = 0;
            AddSprite(Player);
            AddSprite(AlienArsenal);
            BuildAlienGroups();
            BuildBunkers();
        }

        private void BuildAlienGroups()
        {

            BuildAlienGroup(3, AlienData30A, AlienData30B, 30);
            BuildAlienGroup(6, AlienData20A, AlienData20B, 20);
            BuildAlienGroup(9, AlienData20A, AlienData20B, 20);
            BuildAlienGroup(12, AlienData10A, AlienData10B, 10);
            BuildAlienGroup(15, AlienData10A, AlienData10B, 10);

        }

        private void BuildBunkers()
        {
            int xpos = 15;
            for (int i = 0; i < 4; i++)
            {
                var bunker = new Bunker(GameData, this, 7, 3)
                {
                    CollisionController = CollisionController
                };
                bunker.Bounds.Position.X = xpos;
                bunker.Bounds.Position.Y = 31;
                AddSprite(bunker);
                xpos += 30;
            }
        }

        private void BuildAlienGroup(int y, string dataA, string dataB, int worth)
        {

            var group = new AlienGroup(GameData, this, 20, Direction.Right)
            {
                PauseRebuild = true,
                LayerOrder = 1,
                Frequency = 10,
                TrajectoryController = AlienController,
                CollisionController = CollisionController // AlienGroupCollisionController
            };
            group.Bounds.Position.Y = y;
            group.RendererData.StepX = 1;
            for (int i = 0; i < 11; i++)
            {
                BuildAlien(group, dataA, dataB, 8 * i + 5, 0, worth);
            }
            group.PauseRebuild = false;
            group.RebuildData();
            group.Bounds.Size.Lock();
            AlienArsenal.AddAlienGroup(group);
            AddSprite(group);
        }

        private void BuildAlien(AlienGroup group, string dataA, string dataB, int xposition, int yposition, int worth)
        {
            var alien = new Alien(GameData, this, 20, Direction.Right, new Size(6, 2), worth)
            {
                LayerOrder = 1,
                Frequency = 10,
                TrajectoryController = AlienController,
                Type = SpriteTypes.ALIEN
            };
            alien.SetData(dataA);
            alien.SetData2(dataB);
            alien.Bounds.Position.X = xposition;
            alien.Bounds.Position.Y = yposition;
            group.Add(alien);
        }

        public override void Update()
        {
            base.Update();
            if (!IsGameOver)
            {
                if (GameData.StopGame)
                {
                    MainAudioPlayer.Pause();
                    if (Playerboard.HasLife())
                    {
                        // Player lost a life but has another
                        Thread.Sleep(1000);
                        MainAudioPlayer.Play();
                        GameData.StopGame = false;
                    }
                    else
                    {
                        // Player is dead, we can move on now
                        var sprites = QuerySprites(x => x is Bullet || x is AlienGroup || x is AlienArsenal);
                        base.RemoveSprites(sprites);
                        AlienArsenal.Stop();
                        AlienArsenal = null;
                        IsGameOver = true;
                        EndScene();
                        return;
                    }
                }
                var gameData = GameData as SIGameData;
                if (gameData.PlayerDeath)
                {
                    GameData.StopGame = true;
                    gameData.PlayerDeath = false;
                    var bullets = QuerySprites(x => x is Bullet);
                    bullets.All(x => x.MarkDelete = true);
                    RemoveSprites(bullets);
                }

                if (!HasSprite(x => x is AlienGroup))
                {
                    Thread.Sleep(2000);
                    AlienArsenal.Clear();
                    BuildAlienGroups();
                }

            }
        }

        public override void GameLoopStarted()
        {
            MainAudioPlayer.Play();
        }

        public override void GameLoopStopped()
        {
            MainAudioPlayer.Pause();
            GameData.IsActive = false;
        }

        public override void OnEndScene()
        {
            base.OnEndScene();
            
        }

        public override void Draw()
        {
            base.Draw();
            Player.Direction = Direction.None;
        }

        #region Key Events

        public override void OnDownKeyDown()
        {
            if (!IsGameOver)
                Player.FireAtWill(TrajectoryController, CollisionController);
        }

        public override void OnLeftKeyDown()
        {
            if (!IsGameOver)
                Player.Direction = Direction.Left;
        }

        public override void OnRightKeyDown()
        {
            if (!IsGameOver)
                Player.Direction = Direction.Right;
        }

        public override void OnUpKeyDown()
        {
            if (!IsGameOver)
                Player.Direction = Direction.Up;
        }

        public override void OnSpaceKeyDown()
        {
            if (!IsGameOver)
                Player.FireAtWill(TrajectoryController, CollisionController);
        }

        #endregion

    }
}
