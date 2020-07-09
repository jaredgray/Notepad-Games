
namespace NPPGames.Core.Primitives
{
    public class Bounds
    {
        public Bounds()
        {
            Position = new Vector2D();
            OffsetPosition = new Vector2D();
            Size = new Size();
        }
        public Vector2D Position { get; set; }
        public Vector2D OffsetPosition { get; set; }
        public Vector2D GetAbsolutePosition()
        {
            return Position + OffsetPosition;
        }
        public Size Size 
        {
            get { return _size; }
            set
            {
                if (null != _size && _size.IsLocked())
                    return;
                _size = value;
            }
        }
        private Size _size;

        public void MoveLeft(int amount)
        {
            this.Position.X -= amount;
        }
        public void MoveRight(int amount)
        {
            this.Position.X += amount;
        }
        public void MoveUp(int amount)
        {
            this.Position.Y -= amount;
        }
        public void MoveDown(int amount)
        {
            this.Position.Y += amount;
        }

        /// <summary>
        /// Determines if this bounds intersects with another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IntersectsWith(Bounds other)
        {
            return (other.Position.X < this.Position.X + this.Size.Width) &&
            (this.Position.X < (other.Position.X + other.Size.Width)) &&
            (other.Position.Y < this.Position.Y + this.Size.Height) &&
            (this.Position.Y < other.Position.Y + other.Size.Height);
        }
    }
}
