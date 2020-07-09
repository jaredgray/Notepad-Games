using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NPPGames.Core.Infrastructure
{
    public class Win32Console
    {
        /// <summary>
        /// Other open operations can be performed on the console screen buffer for read access.
        /// </summary>
        public const uint FILE_SHARE_READ = 0x00000001;
        /// <summary>
        /// Other open operations can be performed on the console screen buffer for write access.
        /// </summary>
        public const uint FILE_SHARE_WRITE = 0x00000002;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const int CONSOLE_TEXTMODE_BUFFER = 1;
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        /// <summary>
        /// Creates a console screen buffer.
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the console screen buffer. For a list of access rights, see "Console Buffer Security and Access Rights".</param>
        /// <param name="dwShareMode">This parameter can be zero, indicating that the buffer cannot be shared</param>
        /// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle can be inherited by child processes. If lpSecurityAttributes is NULL, the handle cannot be inherited. 
        ///     The lpSecurityDescriptor member of the structure specifies a security descriptor for the new console screen buffer. If lpSecurityAttributes is NULL, the console screen buffer gets a default security descriptor. 
        ///     The ACLs in the default security descriptor for a console screen buffer come from the primary or impersonation token of the creator.</param>
        /// <param name="dwFlags">The type of console screen buffer to create. The only supported screen buffer type is CONSOLE_TEXTMODE_BUFFER.</param>
        /// <param name="lpScreenBufferData">Reserved; should be NULL. (IntPtr.Zero)</param>
        /// <returns>If the function succeeds, the return value is a handle to the new console screen buffer. If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</returns>
        /// More Info: https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(
                UInt32 dwDesiredAccess,
                UInt32 dwShareMode,
                IntPtr lpSecurityAttributes,
                int dwFlags,
                IntPtr lpScreenBufferData
            );
        /// <summary>
        /// Sets the specified screen buffer to be the currently displayed console screen buffer.
        /// </summary>
        /// <param name="Handle">A handle to the console screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleActiveScreenBuffer(IntPtr Handle);


        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int HIDE = 0;
        public const int MAXIMIZE = 3;
        public const int MINIMIZE = 6;
        public const int RESTORE = 9;

        /*
         BOOL WINAPI SetConsoleWindowInfo(
  _In_       HANDLE     hConsoleOutput,
  _In_       BOOL       bAbsolute,
  _In_ const SMALL_RECT *lpConsoleWindow
);
         */

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool bAbsolute, [In] ref SmallRect lpConsoleWindow );
    }
}
