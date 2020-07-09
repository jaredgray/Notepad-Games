using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using NPPGames;
using NPPGames.Core.Renderers;
using NPPGames.Core;

namespace NotepadEmulator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //TestInput();
            RunGame();
        }

        static void RunGame()
        {
            Logger.APPEND_LOG = false;
            var logger = new Logger(@"C:\Temp\ConsoleLogger.txt");
            logger.StartLogging();
            var renderer = new ConsoleRenderer(logger);
            Kbg.NppPluginNET.Main.Main2(renderer);
            while (true)
            {

            }
        }

        static void TestInput()
        {
            var keyboard = new Keyboard();
            keyboard.ListenTo(Win32.Win32Keyboard.VirtualKeyStates.VK_RETURN);
            while(true)
            {
                keyboard.Update();
                if (Win32.Win32Keyboard.IsKeyPressed(Win32.Win32Keyboard.VirtualKeyStates.VK_RETURN))
                    break;
            }
        }
    }
}
