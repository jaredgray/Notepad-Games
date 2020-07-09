using System;
using NPPGames.Core;
using NPPGames.Core.Primitives;

namespace NPPGames.Games.SpaceInvaders.Scenes
{
    public class SIScene : Scene
    {
        public SIScene(IRenderer renderer, Logger logger, Size size, GameData gameData) : base(renderer, logger, size, gameData)
        {
            
        }

        public event EventHandler SceneComplete;
        protected void EndScene()
        {
            SceneComplete?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnLeftKeyDown() { }
        public virtual void OnUpKeyDown() { }
        public virtual void OnRightKeyDown() { }
        public virtual void OnDownKeyDown() { }
        public virtual void OnSpaceKeyDown() { }
    }
}
