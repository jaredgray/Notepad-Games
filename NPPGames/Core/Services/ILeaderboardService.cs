namespace NPPGames.Core.Services
{
    public interface ILeaderboardService
    {
        Leaderboard GetLeaderboardByGameId(string gameId);
        void SavePlayer(LeaderboardPlayer leaderboardPlayer);
    }
}