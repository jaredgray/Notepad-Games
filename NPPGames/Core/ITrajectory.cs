using NPPGames.Core.Primitives;

namespace NPPGames.Core
{
    public enum EdgeScreenHandling
    {
        Disappear,
        ReverseDirection,
        Stop
    }
    public interface ITrajectory
    {
        /// <summary>
        /// Determines how quickly this moves - 1 being every time the screen updates, 2 being every other update - etc.
        /// </summary>
        int Frequency { get; }
        /// <summary>
        /// thi direction the trajectory travels
        /// </summary>
        Direction Direction { get; set; }
        /// <summary>
        /// determines what happens when the edge of the screen is met
        /// </summary>
        EdgeScreenHandling EdgeOfScreenCondition { get; }
        /// <summary>
        /// data used by the rendering system. If this is not initialized by the class that owns it, the control system will initialize it
        /// </summary>
        TrajectoryRendererData TrajectoryRendererData { get; set; }
    }
}
