using System;
using System.Collections.Generic;
using System.Linq;
using NPPGames.Core.Primitives;

namespace NPPGames.Core
{
    public abstract class Scene
    {
        public static bool DO_LOG = false;

        public Scene(IRenderer renderer, Logger logger, Size size, GameData gameData)
        {
            Renderer = renderer;
            Logger = logger;
            GameData = gameData;
            Sprites = new List<Sprite>();
            Size = size;
            InitializeData();
        }

        #region public properties

        public Size Size { get; set; }

        public Playerboard Playerboard { get; private set; }

        public IRenderer Renderer { get; set; }

        #endregion

        #region private variables

        private List<byte> Data { get; set; }
        private List<Sprite> Sprites { get; set; }

        #endregion

        #region protected variables 

        protected Logger Logger { get; set; }

        protected GameData GameData { get; set; }

        #endregion

        #region protected overridable methods


        #endregion

        #region public methods

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {
            ClearData();
            DrawSprites();
            this.Renderer.PreRenderFrame();
            this.Renderer.RenderFrame(Data);
        }

        public void ClearSprites()
        {
            this.Sprites.Clear();
        }

        public bool HasSprite(Func<Sprite, bool> evaluator)
        {
            return Sprites.Any(x => evaluator(x));
        }

        public IEnumerable<Sprite> QuerySprites(Func<Sprite, bool> evaluator)
        {
            return Sprites.Where(x => evaluator(x));
        }

        public void AddSprite(Sprite sprite)
        {
            this.Sprites.Add(sprite);
        }

        public void AddSprites(IEnumerable<Sprite> sprites)
        {
            foreach (var sprite in sprites)
            {
                this.Sprites.Add(sprite);
            }
        }

        public void RemoveSprite(Sprite sprite)
        {
            this.Sprites.Remove(sprite);
        }

        public void RemoveSprites(IEnumerable<Sprite> sprites)
        {
            if(sprites.Any())
            {
                for (int i = sprites.Count() - 1; i > 0; i--)
                {
                    var victim = sprites.ElementAt(i);
                    RemoveSprite(victim);
                }
            }
        }

        public void SetPlayerboard(Playerboard board)
        {
            if (null != Playerboard)
            {
                RemoveSprite(Playerboard);
            }

            Playerboard = board;
            AddSprite(Playerboard);
        }

        public void ResumeGame()
        {
            foreach (var sprite in Sprites)
            {
                sprite.ResumeGame();
            }
        }

        public virtual void OnStartScene(Scene lastScene) { }
        public virtual void OnEndScene() 
        {
            this.ClearSprites();
        }

        #endregion

        #region Scene overridable interface 

        public virtual void InitializeScene()
        {

        }

        public virtual void GameLoopStarted()
        {

        }
        public virtual void GameLoopStopped()
        {

        }

        #endregion 

        #region Game Loop Drawing Methods

        private void InitializeData()
        {
            Data = new List<byte>(Size.Width * Size.Height);
            for (int i = 0; i < (Size.Width * Size.Height); i++)
            {
                Data.Add((byte)' ');
            }
        }

        private void ClearData()
        {
            for (int i = 0; i < (Size.Width * Size.Height); i++)
            {
                Data[i] = (byte)' ';
            }
        }

        private void ClampSprite(Sprite sprite)
        {
            if (sprite.Bounds.Position.X < 0)
                sprite.Bounds.Position.X = 0;
            if (sprite.Bounds.Position.X + sprite.Bounds.Size.Width > Size.Width)
                sprite.Bounds.Position.X = Size.Width - sprite.Bounds.Size.Width;
            if (sprite.Bounds.Position.Y < 0)
                sprite.Bounds.Position.Y = 0;
            if (sprite.Bounds.Position.Y + sprite.Bounds.Size.Height > Size.Height)
                sprite.Bounds.Position.Y = Size.Height - sprite.Bounds.Size.Height;
        }

        private void ProcessTrajectorySprite(Sprite sprite)
        {
            if (null != sprite.TrajectoryController)
                sprite.TrajectoryController.HandleSprite(sprite);
        }
        private void ProcessCollisionSprite(Sprite sprite, IEnumerable<Sprite> collidables)
        {
            if (sprite is ICollider && null != sprite.CollisionController)
                sprite.CollisionController.HandleSprite(sprite, collidables);
        }

        private void DrawSprites()
        {
            var collidables = Sprites.Where(x => x is ICollider);
            foreach (var sprite in Sprites.OrderBy(x => x.LayerOrder))
            {
                ProcessTrajectorySprite(sprite);
                if (sprite.MarkDelete)
                    continue;

                ProcessCollisionSprite(sprite, collidables);
                if (sprite.MarkDelete)
                    continue;

                sprite.Update();
                // clamp the sprite's position so that it doesn't go out of bounds
                ClampSprite(sprite);
                for (int y = 0; y < sprite.Bounds.Size.Height; y++)
                {
                    for (int x = 0; x < sprite.Bounds.Size.Width; x++)
                    {
                        var c = sprite.GetCharAt((y * sprite.Bounds.Size.Width) + x);
                        var ypos = (y * Size.Width) + (sprite.Bounds.Position.Y * Size.Width);
                        var xpos = x + sprite.Bounds.Position.X;
                        Data[ypos + xpos] = (byte)c;
                    }
                }
            }

            Sprites.RemoveAll(x => x.MarkDelete);

        }

        #endregion
    }
}
