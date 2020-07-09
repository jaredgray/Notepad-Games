using System.Collections.Generic;
using System.Linq;

namespace NPPGames.Core
{
    public class CollisionController : ICollisionController
    {
        public CollisionController()
        {
            
        }

        public void HandleSprite(Sprite sprite, IEnumerable<Sprite> others)
        {
            foreach (var candidate in others)
            {
                var candidateCollider = candidate as ICollider;
                // stop hitting yourself.. stop hitting yourself
                if (candidate == sprite || candidateCollider.HasCollided)
                    continue;

                if(sprite.CollidesWith(candidate, out var collided))
                {
                    var spriteCollider = collided as ICollider;
                    if (sprite.MarkDelete || spriteCollider.HasCollided)
                        return;

                    if (!spriteCollider.CollidesWithTypes.Any(x => x == candidate.Type))
                        return;

                    spriteCollider.HasCollided = candidateCollider.HasCollided = true;
                  
                    if (candidateCollider.CollisionBehavior == CollisionBehavior.Remove)
                    {
                        candidate.MarkDelete = true;
                        candidateCollider.OnCollision(collided);
                    }
                    else if (candidateCollider.CollisionBehavior == CollisionBehavior.RunDestroySequence)
                    {
                        candidateCollider.RunDestroySequence();
                        candidateCollider.OnCollision(collided);
                    }
                    else if (spriteCollider.CollisionBehavior == CollisionBehavior.Custom)
                    {
                        candidateCollider.OnCollision(collided);
                    }

                    if (spriteCollider.CollisionBehavior == CollisionBehavior.Remove)
                    {
                        collided.MarkDelete = true;
                        spriteCollider.OnCollision(candidate);
                    }
                    else if(spriteCollider.CollisionBehavior == CollisionBehavior.RunDestroySequence)
                    {
                        spriteCollider.RunDestroySequence();
                        spriteCollider.OnCollision(candidate);
                    }
                    else if(spriteCollider.CollisionBehavior == CollisionBehavior.Custom)
                    {
                        spriteCollider.OnCollision(candidate);
                    }

                   
                   
                    break;
                }
            }
        }
    }
}
