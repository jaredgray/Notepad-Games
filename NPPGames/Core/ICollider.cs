using System.Collections.Generic;

namespace NPPGames.Core
{
    public enum CollisionBehavior
    {
        None,
        Remove,
        RunDestroySequence,
        Custom
    }
    public interface ICollider
    {
        bool HasCollided { get; set; }
        CollisionBehavior CollisionBehavior { get; set; }
        void RunDestroySequence();
        IEnumerable<string> CollidesWithTypes { get; set; }
        void OnCollision(Sprite other);
    }
}
