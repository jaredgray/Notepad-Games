using System;
using System.Collections.Generic;
using NPPGames.Core;

namespace NPPGames.Games.SpaceInvaders
{
    public class Bunker : Sprite, ICollider
    {
        string Character = @" OOOOO 
OOOOOOO
OOO OOO";
        public Bunker(GameData gameData, Scene scene, int width, int height) : base(gameData, scene, width, height)
        {
            base.SetData(Character);
            this.CollidesWithTypes = new List<string> { SpriteTypes.ALIEN_BULLET, SpriteTypes.PLAYER_BULLET };
            this.CollisionBehavior = CollisionBehavior.Custom;
        }

        public bool HasCollided { get { return false; } set { } }
        public CollisionBehavior CollisionBehavior { get; set; }
        public IEnumerable<string> CollidesWithTypes { get; set; }

        public void OnCollision(Sprite other)
        {
            if(other is Bullet)
            {
                var bullet = (other as Bullet);
                var otherposition = other.Bounds.GetAbsolutePosition();
                var thisoffsetposition = Bounds.GetAbsolutePosition();
                var offsetY = otherposition.Y - thisoffsetposition.Y;
                var offsetX = otherposition.X - thisoffsetposition.X;
                var dataindex = (Bounds.Size.Width * offsetY) + offsetX;
                var currentvalue = this.GetCharAt(dataindex);
                if(currentvalue == 'O')
                {
                    SetCharAt(dataindex, '*');
                }
                else if(currentvalue == '*')
                {
                    SetCharAt(dataindex, ' ');
                }
                else
                {
                    bullet.MarkDelete = bullet.HasCollided = false;
                }
            }
            else
            {

            }

            foreach (var c in Data)
            {
                if (c != ' ')
                    return;
            }

            this.MarkDelete = other.MarkDelete = true;
        }

        public void RunDestroySequence()
        {
            throw new NotImplementedException();
        }
    }
}
