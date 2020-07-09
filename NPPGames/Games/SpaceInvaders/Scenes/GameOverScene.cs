using System;
using NPPGames.Core;
using NPPGames.Core.Primitives;
using NPPGames.Core.UserControls;
using static Kbg.NppPluginNET.PluginInfrastructure.Win32.Win32Keyboard;

namespace NPPGames.Games.SpaceInvaders.Scenes
{
    public class GameOverScene : SIScene
    {
        public GameOverScene(IRenderer renderer, Logger logger, Size size, GameData gameData) : base(renderer, logger, size, gameData)
        {
        }

        public override void InitializeScene()
        {
            base.InitializeScene();
            TimeToExit = DateTime.Now.AddSeconds(6);
            BuildCharacters();
        }
        DateTime TimeToExit;
        public Textbox Name { get; set; }
        public Sprite Ok { get; set; }
        private void BuildCharacters()
        {
            var gameover = new Sprite(GameData, this, 9, 1);
            gameover.Bounds.Position.X = 55;
            gameover.Bounds.Position.Y = 10;
            gameover.SetData("GAME OVER");
            AddSprite(gameover);

            var prompt = new Sprite(GameData, this, 16, 1);
            prompt.Bounds.Position.X = 50;
            prompt.Bounds.Position.Y = 12;
            prompt.SetData("ENTER YOUR NAME:");
            AddSprite(prompt);

            Name = new Textbox(GameData, this, 20);
            Name.Bounds.Position.X = 48;
            Name.Bounds.Position.Y = 14;
            var name = GameData.Session.Player.Name;
            if (null != name)
                Name.Value = name;

            AddSprite(Name);

            // this needs to be a button
            Ok = new Sprite(GameData, this, 4, 1);
            Ok.Bounds.Position.X = 57;
            Ok.Bounds.Position.Y = 15;
            Ok.SetData(" OK ");
            AddSprite(Ok);
        }

        public int Focus { get; set; }

        public override void OnStartScene(Scene lastScene)
        {
            base.OnStartScene(lastScene);
            this.AddSprites(lastScene.QuerySprites(x => true));
        }

        public override void Update()
        {
            base.Update();

            var newkeys = GameData.Keyboard.GetNewKeysDown();
            foreach (var key in newkeys)
            {
                if (Focus == 0)
                {
                    if (key == VirtualKeyStates.VK_TAB)
                    {
                        Focus = 1;
                        Name.Enabled = false;
                        Ok.SetData("[OK]");
                    }
                }
                else
                {
                    if (key == VirtualKeyStates.VK_TAB)
                    {
                        Focus = 0;
                        Name.Enabled = true;
                        Ok.SetData(" OK ");
                    }
                    if (key == VirtualKeyStates.VK_RETURN)
                    {
                        if(!string.IsNullOrEmpty(Name.Value))
                        {
                            GameData.Session.Player.Name = Name.Value;
                            GameData.Session.Player.LastGame = DateTime.Now;
                            // TODO: Player should exist across games.. we should be adding a new GameStats (doesn't exist) to the player instead
                            GameData.Session.Player.GamesPlayed++;
                            EndScene();
                        }
                    }
                }

            }
        }
    }
}
