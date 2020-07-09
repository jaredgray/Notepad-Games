using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using NPPGames.Core.Primitives;
using static NPPGames.Core.Infrastructure.Win32Console;

namespace NPPGames.Core.Renderers
{
    public class ConsoleRenderer : IRenderer
    {
        public static bool DO_LOG = false;
        public ConsoleRenderer(Logger logger)
        {
            Logger = logger;
        }

        private Logger Logger { get; set; }

        IntPtr ConsoleWindowHandle { get; set; }

        public Size SceneSize { get; set; }
        IntPtr ScreenBufferHandle { get; set; }
        SafeFileHandle SafeScreenBufferHandle { get; set; }
        public CharInfo[] CharInfoOutputBuffer { get; set; }
        public SmallRect ViewportSize;
        //public SmallRect ConsoleWindowSize;
        
        public void Initialize(Size sceneSize)
        {
            this.SceneSize = sceneSize;
            ScreenBufferHandle = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            SetConsoleActiveScreenBuffer(ScreenBufferHandle);
            SafeScreenBufferHandle = new SafeFileHandle(ScreenBufferHandle, true);
            Console.ForegroundColor = ConsoleColor.Green;
            CharInfoOutputBuffer = new CharInfo[SceneSize.Width * SceneSize.Height];
            ViewportSize = new SmallRect() { Left = 0, Top = 0, Right = (short)SceneSize.Width, Bottom = (short)SceneSize.Height };
            ConsoleWindowHandle = GetConsoleWindow();
            ShowWindow(ConsoleWindowHandle, MAXIMIZE);
        }

        public void InitializeFrameBuffer()
        {
        }

        public void RenderFrame(IEnumerable<byte> buffer)
        {
            for (int y = 0; y < SceneSize.Height; y++)
            {
                for (int x = 0; x < SceneSize.Width; x++)
                {
                    var index = (y * SceneSize.Width) + x;
                    CharInfoOutputBuffer[index].Attributes = (short)ConsoleColor.Green;
                    CharInfoOutputBuffer[index].Char.AsciiChar = buffer.ElementAt(index);
                }
            }
            WriteConsoleOutput(SafeScreenBufferHandle, CharInfoOutputBuffer,
              new Coord() { X = (short)SceneSize.Width, Y = (short)SceneSize.Height },
              new Coord() { X = 0, Y = 0 },
              ref ViewportSize);
        }

        public void PreRenderFrame()
        {
            // Clear the console
            Console.Clear();
        }

        public void OnScreenResize(Size newSize)
        {
            if (SceneSize != newSize)
                this.SceneSize = newSize;
        }

    }
}
