using NPPGames.Core;
using NPPGames.Core.Primitives;

namespace NPPGames.Games.SpaceInvaders
{
    public class AlienTrajectoryController : TrajectoryController
    {
        public AlienTrajectoryController(Size gameArea, Logger logger)
            : base(gameArea, logger)
        {

        }

        protected override void HandleScreenEdgeDisappear(Sprite sprite, ITrajectory trajectory)
        {
            //base.HandleScreenEdgeDisappear(sprite, trajectory);
        }

        protected override void OnReverseDirection(Sprite sprite, ITrajectory trajectory)
        {
            base.OnReverseDirection(sprite, trajectory);
            sprite.Bounds.Position.Y += sprite.RendererData.StepY;
        }
    }
}
