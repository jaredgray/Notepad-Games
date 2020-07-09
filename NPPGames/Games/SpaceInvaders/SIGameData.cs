using NPPGames.Core;
using NPPGames.Core.Services;

namespace NPPGames.Games.SpaceInvaders
{
    public class SIGameData : GameData
    {
        public SIGameData(ILeaderboardService leaderboardService, string gameId) : base(leaderboardService, gameId) { }
        public bool PlayerDeath { get; set; }
    }
}
