using System.Collections.Generic;

namespace NPPGames.Core
{
    public interface ICollisionController
    {
        void HandleSprite(Sprite sprite, IEnumerable<Sprite> others);
    }
}
