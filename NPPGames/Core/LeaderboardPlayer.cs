using System;
using System.Collections.Generic;

namespace NPPGames.Core
{
    public class LeaderboardPlayer
    {
        public LeaderboardPlayer()
        {

        }

        public List<int> Scores { get; set; }

        public string Name { get; set; }

        public int GamesPlayed { get; set; }

        public DateTime LastGame { get; set; }

        public string UniqueId { get; set; }
    }
}
