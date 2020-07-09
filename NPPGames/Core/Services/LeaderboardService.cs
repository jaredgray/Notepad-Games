using System.Collections.Generic;
using System.Linq;

namespace NPPGames.Core.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        public LeaderboardService()
        {
            Leaderboards = new List<Leaderboard>();
            // TODO: Load leaderboards
        }

        public List<Leaderboard> Leaderboards { get; set; }


        public Leaderboard GetLeaderboardByGameId(string gameId)
        {
            // load from network
            var board = Leaderboards.FirstOrDefault(x => x.GameId == gameId);
            if (null == board)
            {
                board = new Leaderboard(gameId)
                {

                };
            }
            return board;
        }

        public void SavePlayer(LeaderboardPlayer leaderboardPlayer)
        {
            // save leaderboard to persistant store
        }
    }
}
