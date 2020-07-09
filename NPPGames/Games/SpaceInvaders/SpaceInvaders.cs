using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using NPPGames.Core.Services;
using NPPGames.Games.SpaceInvaders.Scenes;
using NPPGames.Core;

namespace NPPGames.Games.SpaceInvaders
{
    public class SpaceInvaders : Game<SIScene, SIGameData>
    {
        public SpaceInvaders(IRenderer renderer, ILeaderboardService leaderboardService)
            : base(renderer, leaderboardService)
        {
            Scenes = new List<SIScene>();
        }

        List<SIScene> Scenes { get; set; }
        int NextSceneIndex { get; set; }

        public override string GameId => "Space Invaders";

        protected override void ChangeScene()
        {
            if (NextSceneIndex >= Scenes.Count)
                NextSceneIndex = 0;
            var currentScene = Scene;
            var nextScene = Scenes[NextSceneIndex];
            nextScene.InitializeScene();
            nextScene.OnStartScene(currentScene);
            if (null != currentScene)
            {
                currentScene.OnEndScene();
            }


            Scene = nextScene;

            ++NextSceneIndex;
            Scene.SceneComplete += Scene_SceneComplete;
            Scene.InitializeScene();
        }

        private void Scene_SceneComplete(object sender, EventArgs e)
        {
            Scene.SceneComplete -= Scene_SceneComplete;
            // change the current scene
            ChangeScene();
        }

        protected override void InitializeInternal()
        {
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_LEFT);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_UP);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_RIGHT);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_DOWN);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_SPACE);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_RETURN);
            //GameData.Keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_ESCAPE);
            GameData.Keyboard.ListenToAllKeys();
            Scenes.Add(new NewGameScene(Renderer, Logger, GameSize, GameData));
            Scenes.Add(new PlayGameScene(Renderer, Logger, GameSize, GameData));
            Scenes.Add(new GameOverScene(Renderer, Logger, GameSize, GameData));
        }

        protected override void StartGameLoop()
        {

        }

        protected override void StopGameLoop()
        {

        }

        protected override void Update()
        {
            /*             
            yeah, this sucks..unfortunately couldn't find a way to listen to key events.
            typical way of doing this by pinvoking the SetWindowsHookEx does not work.
            Instead it just crashes Notepad++. Theres a good chance that it has to do
            with the apartment state or some kind of threading issue.

            also tried turning on the macro recorder from the ScintillaGateway and 
            listening to messages trying to pick out the key characters but that
            doesn't work either. it appeared that since we were writing a lot of 
            spaces to the screen, Scintilla was calling back as if the user was
            pressing the space bar. the arrow heys seemed to work fine but is not
            enough input to play a game

            so here we are.
             */
            if (GameData.Keyboard.IsKeyDown(Win32.Win32Keyboard.VirtualKeyStates.VK_LEFT))
            {
                Scene?.OnLeftKeyDown();
            }
            if (GameData.Keyboard.IsKeyDown(Win32.Win32Keyboard.VirtualKeyStates.VK_RIGHT))
            {
                Scene?.OnRightKeyDown();
            }
            if (GameData.Keyboard.IsKeyDown(Win32.Win32Keyboard.VirtualKeyStates.VK_UP))
            {
                Scene?.OnUpKeyDown();
            }
            if (GameData.Keyboard.IsKeyDown(Win32.Win32Keyboard.VirtualKeyStates.VK_DOWN))
            {
                Scene?.OnDownKeyDown();
            }
            if (GameData.Keyboard.IsKeyDown(Win32.Win32Keyboard.VirtualKeyStates.VK_SPACE))
            {
                Scene?.OnSpaceKeyDown();
            }
        }

        protected override void CreateGameData(ILeaderboardService leaderboardService)
        {
            GameData = new SIGameData(leaderboardService, GameId)
            {
            };
        }
    }
}
