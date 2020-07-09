using NPPGames.Core.Services;

namespace NPPGames.Core
{
    public class GameData
    {
        public static int FrameSequence;
        public GameData(ILeaderboardService leaderboardService, string gameId)
        {
            Keyboard = new Keyboard();
            Session = new Session(leaderboardService, gameId);
        }
        public bool StopGame { get; set; }
        public bool IsAlive { get; set; }
        public bool IsActive { get; set; }
        public bool IsComplete { get; set; }
        public int Level { get; set; }
        public Keyboard Keyboard { get; }

        public Session Session { get; set; }

    }
}
