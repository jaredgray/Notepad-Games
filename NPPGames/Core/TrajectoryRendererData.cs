
namespace NPPGames.Core
{
    public class TrajectoryRendererData
    {
        public TrajectoryRendererData()
        {
        }
        /// <summary>
        /// Property to be used only by the trajectory controller
        /// </summary>
        public int IterationsSinceLastMovement { get; set; }
    }
}
