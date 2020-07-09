
namespace NPPGames.Core
{
    public interface IShooter
    {
        void FireAtWill(TrajectoryController bulletController, ICollisionController collisionController);
    }
}
