using System.Collections.Generic;
using System.Linq;

namespace NPPGames.Core
{
    public class Leaderboard
    {
        public Leaderboard(string gameId)
        {
            GameId = gameId;
            this.Leaders = new List<LeaderboardPlayer>();
        }

        public string GameId { get; set; }
        private List<LeaderboardPlayer> Leaders { get; set; }


        public LeaderboardPlayer GetPlayerByUniqueId(string uniqueid)
        {
            return Leaders.FirstOrDefault(x => x.UniqueId == uniqueid);
        }

        public void AddPlayer(LeaderboardPlayer player)
        {
            var existing = GetPlayerByUniqueId(player.UniqueId);
            if (null == existing)
            {
                Leaders.Add(player);
            }
            else if (existing == player) { }
            else
            {
                // Merge the existing user
            }
        }
    }
}
