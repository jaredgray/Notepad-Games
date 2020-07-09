using System.IO;

namespace NPPGames
{
    public class Logger
    {
        public static bool APPEND_LOG = true;
        public Logger(string logPath)
        {
            LogPath = logPath;
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
        }
        public string LogPath { get; set; }

        StreamWriter sw = null;

        public void StartLogging()
        {
            if(null == sw)
            {
                sw = new StreamWriter(LogPath, APPEND_LOG)
                {
                    AutoFlush = true
                };
            }
        }


        public void StopLogging()
        {
            sw.Close();
            sw.Dispose();
            sw = null;
        }

        public void WriteLine(string text)
        {
            if(null != sw)
            {
                try
                {
                    sw.WriteLine(text);
                }
                catch
                {
                }
            }
        }
    }
}
