using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using NPPGames;
using NPPGames.Core;
using NPPGames.Core.Renderers;
using NPPGames.Core.Services;
using NPPGames.Games.SpaceInvaders;

namespace Kbg.NppPluginNET
{
    public class Main
    {
        internal const string PluginName = "NPPGames";
        static IScintillaGateway editor;
        static INotepadPPGateway notepad;
        static Logger logger;
        public static readonly int COMMAND_SPACEINVADERS = 0;

        static IntPtr CurrentWindowId;
        static Dictionary<IntPtr, SpaceInvaders> spaceInvaderGames;
        static NppRenderer NppRenderer;


        static Main()
        {
            spaceInvaderGames = new Dictionary<IntPtr, SpaceInvaders>();
            editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            notepad = new NotepadPPGateway();
            logger = new Logger(@"C:\Temp\NPPGamesLog.txt");
            NppRenderer = new NppRenderer(notepad, editor, logger);
            //editor.StartRecord();
            logger.StartLogging();

        }

        /// <summary>
        /// entry point for an application other than Notepad++. this allows another application to run and even render the game
        /// </summary>
        /// <param name="pEditor"></param>
        /// <param name="pNotepad"></param>
        public static void Main2(IRenderer renderer)
        {
            var service = new LeaderboardService();
            var game = new SpaceInvaders(renderer, service);
            game.GameOver += Game_GameOver;
            spaceInvaderGames.Add(IntPtr.Zero, game);
            game.Initialize();
            game.Run();
        }

        private static void Game_GameOver(object sender, EventArgs e)
        {
            var currentWindowid = editor.GetDocPointer();
            logger.WriteLine($"{currentWindowid}-{DateTime.Now} Game Over Invoked");
            if (spaceInvaderGames.ContainsValue((SpaceInvaders)sender))
            {
                var gameentry = spaceInvaderGames.FirstOrDefault(x => x.Value.Equals(sender));
                spaceInvaderGames.Remove(gameentry.Key);

                var gameWindowid = gameentry.Key;
                if (gameWindowid == currentWindowid)
                {
                    logger.WriteLine($"{currentWindowid}-{DateTime.Now} Game Over");
                    notepad.CloseCurrent();
                }
            }
        }

        /// <summary>
        /// handles messages sent from Notepad++ or another application
        /// </summary>
        /// <param name="notification"></param>
        public static void OnNotification(ScNotification notification)
        {
            if (spaceInvaderGames.Count == 0)
                return;

            // get document pointer and notify game
            var newWindowId = editor.GetDocPointer();

            //if (notification.Header.Code == (uint)NppMsg.NPPN_FILEBEFORECLOSE)
            //{
            //    if (spaceInvaderGames.TryGetValue(newWindowId, out var game))
            //    {
            //        game.Quit = true;
            //        game.GameData.IsActive = game.GameData.IsAlive = false;
            //        notepad.CloseCurrent();
            //    }
            //}
            if (newWindowId != CurrentWindowId)
            {
                // TODO: when new games are added the below code should be abstracted so we don't have to check a bunch of different dictionaries for different games
                if (spaceInvaderGames.TryGetValue(CurrentWindowId, out var game))
                {
                    if (game.GameData.IsActive && !game.GameData.StopGame)
                    {
                        // Pause the game
                        logger.WriteLine($"{CurrentWindowId}-{DateTime.Now} Pausing Game");
                        game.GameData.IsActive = false;
                    }
                }
            }
            else if (newWindowId == CurrentWindowId)
            {
                // TODO: when new games are added the below code should be abstracted so we don't have to check a bunch of different dictionaries for different games
                if (spaceInvaderGames.TryGetValue(newWindowId, out var game))
                {
                    if (!game.GameData.IsActive && !game.GameData.StopGame)
                    {
                        // Un-Pause the game
                        logger.WriteLine($"{newWindowId}-{DateTime.Now} Resuming Game");
                        game.GameData.IsActive = true;
                    }
                }
            }

            //var action = "";
            //Try(() => action = Enum.GetName(typeof(NppMsg), notification.Header.Code), false);
            //if(string.IsNullOrEmpty(action))
            //    Try(() => action = Enum.GetName(typeof(SciMsg), notification.Header.Code), false);


            //if (spaceInvaderGames.TryGetValue(newWindowId, out var game))
            //{
            //    /*
            //     1003 - 2667773762896 - NPPN_FILEBEFORECLOSE - -2147483648 - 1638448
            //     */
            //    if (notification.Header.Code == (uint)NppMsg.NPPN_FILEBEFORECLOSE)
            //    {
            //        shootergame.NotifyBeforeWindowClose(docpointer);
            //    }
            //}

            /* 
             * 4294966745 is the code sent just before the window changes
             * 
             * 4294966745 - 2481992418880 -  - 0 - 459584
             * */
            //var scimsg = Enum.GetName(typeof(SciMsg), notification.Header.Code);
            //Try(() => logger.WriteLine($"{notification.Header.Code} - {scimsg} - {notification.lParam}, {notification.wParam}, {notification.character}, {notification.Character}, {notification.Header.IdFrom}"), false);

            CurrentWindowId = newWindowId;
        }

        public static void CommandMenuInit()
        {
            PluginBase.SetCommand(COMMAND_SPACEINVADERS, "Play Space Invaders", PlaySpaceInvadersGame, new ShortcutKey(false, false, false, Keys.None));
            //PluginBase.SetCommand(COMMAND_STARTLOGGING, "DEV - Start Logging", BeginLogging, new ShortcutKey(false, false, false, Keys.None));
            //PluginBase.SetCommand(COMMAND_ENDLOGGING, "DEV - END Logging", EndLogging, new ShortcutKey(false, false, false, Keys.None));
        }

        internal static void SetToolBarIcon()
        {
            //toolbarIcons tbIcons = new toolbarIcons();
            //tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            //IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            //Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            //Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            //Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
        }

        #region Logging

        internal static void BeginLogging()
        {
            Try(() => logger.StartLogging());
        }

        internal static void EndLogging()
        {
            Try(() => logger.Dispose());
        }

        #endregion

        private static void PlaySpaceInvadersGame()
        {
            var service = new LeaderboardService();
            var game = new SpaceInvaders(NppRenderer, service);
            game.GameOver += Game_GameOver;
            game.Initialize();
            var windowId = editor.GetDocPointer();
            spaceInvaderGames.Add(windowId, game);
            game.Run();
        }

        private static void Try(Action action, bool showError = true)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (showError)
                    Fail(ex);
            }
        }

        private static void Fail(Exception ex)
        {
            notepad.FileNew();
            var exmsg = ex.ToString();
            editor.AppendText(exmsg.Length, exmsg);
            editor.NewLine();
        }

    }
}