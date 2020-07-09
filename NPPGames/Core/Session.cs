using System;
using NPPGames.Core.Infrastructure;
using NPPGames.Core.Services;

namespace NPPGames.Core
{
    public class Session
    {
        public Session(ILeaderboardService leaderboardService, string gameId)
        {
            // also noted int MachineInfo - this might be the most difficult thing to port. See MachineInfo for details
            SessionBeginTime = DateTime.Now;
            LeaderboardService = leaderboardService;
            UserId = MachineInfo.GetMacAddress();
            var leaderboard = leaderboardService.GetLeaderboardByGameId(gameId);
            Player = leaderboard.GetPlayerByUniqueId(UserId);
            if (null == Player)
                Player = new LeaderboardPlayer() { UniqueId = UserId };
            leaderboard.AddPlayer(Player);
        }
        public ILeaderboardService LeaderboardService { get; set; }
        public DateTime SessionBeginTime { get; set; }
        public string UserId { get; set; }
        public LeaderboardPlayer Player { get; set; }

        public void Save()
        {
            LeaderboardService.SavePlayer(Player);
        }
    }
}
