using NPPGames.Core.Primitives;

namespace NPPGames.Core
{
    public class TrajectoryController : IUpdateSpriteHandler
    {
        public TrajectoryController(Size gameArea, Logger logger)
        {
            GameArea = gameArea;
        }
        protected Size GameArea { get; set; }
        protected Logger Logger { get; set; }

        protected virtual void HandleScreenEdgeReverseDirection(Sprite sprite, ITrajectory trajectory)
        {
            if (sprite.Bounds.Position.X < 0)
            {
                sprite.Bounds.Position.X += sprite.RendererData.StepX;
                trajectory.Direction = Direction.Right;
                OnReverseDirection(sprite, trajectory);
            }
            else if (sprite.Bounds.Position.X + sprite.Bounds.Size.Width > GameArea.Width)
            {
                sprite.Bounds.Position.X -= sprite.RendererData.StepX;
                trajectory.Direction = Direction.Left;
                OnReverseDirection(sprite, trajectory);
            }
            else if (sprite.Bounds.Position.Y < 0)
            {
                sprite.Bounds.Position.Y += sprite.RendererData.StepY;
                trajectory.Direction = Direction.Down;
                OnReverseDirection(sprite, trajectory);
            }
            else if (sprite.Bounds.Position.Y + sprite.Bounds.Size.Height > GameArea.Height)
            {
                sprite.Bounds.Position.Y -= sprite.RendererData.StepY;
                trajectory.Direction = Direction.Up;
                OnReverseDirection(sprite, trajectory);
            }
        }

        protected virtual void OnReverseDirection(Sprite sprite, ITrajectory trajectory)
        {

        }


        protected virtual void HandleScreenEdgeDisappear(Sprite sprite, ITrajectory trajectory)
        {
            // NEXT: delete parts of the sprite as it exits the screen before just deleting it
            if (sprite.Bounds.Position.X < 0)
                sprite.MarkDelete = true;
            if (sprite.Bounds.Position.X + sprite.Bounds.Size.Width > GameArea.Width)
                sprite.MarkDelete = true;
            if (sprite.Bounds.Position.Y < 0)
                sprite.MarkDelete = true;
            if (sprite.Bounds.Position.Y + sprite.Bounds.Size.Height > GameArea.Height)
                sprite.MarkDelete = true;
        }

        public void HandleSprite(Sprite sprite)
        {
            if (sprite is ITrajectory)
            {
                var trajectory = sprite as ITrajectory;
                if (null == trajectory.TrajectoryRendererData)
                    trajectory.TrajectoryRendererData = new TrajectoryRendererData();
                if (trajectory.TrajectoryRendererData.IterationsSinceLastMovement == trajectory.Frequency)
                {
                    trajectory.TrajectoryRendererData.IterationsSinceLastMovement = 0;
                    if (trajectory.Direction == Primitives.Direction.Up)
                    {
                        sprite.Bounds.Position.Y -= sprite.RendererData.StepY;
                    }
                    else if (trajectory.Direction == Direction.Right)
                    {
                        sprite.Bounds.Position.X += sprite.RendererData.StepX;
                    }
                    else if (trajectory.Direction == Direction.Down)
                    {
                        sprite.Bounds.Position.Y += sprite.RendererData.StepY;
                    }
                    else if (trajectory.Direction == Direction.Left)
                    {
                        sprite.Bounds.Position.X -= sprite.RendererData.StepX;
                    }
                    if (trajectory.EdgeOfScreenCondition == EdgeScreenHandling.Disappear)
                        HandleScreenEdgeDisappear(sprite, trajectory);
                    else if (trajectory.EdgeOfScreenCondition == EdgeScreenHandling.ReverseDirection)
                        HandleScreenEdgeReverseDirection(sprite, trajectory);
                }
                ++trajectory.TrajectoryRendererData.IterationsSinceLastMovement;
            }
        }

        //public void HandleSprites(IEnumerable<Sprite> sprites)
        //{
        //    var trajectorysprites = sprites.Where(x => x is ITrajectory);
        //    foreach (var sprite in trajectorysprites)
        //    {
        //        var trajectory = sprite as ITrajectory;
        //        if(trajectory.Direction == Primitives.Direction.Up)
        //        {
        //            sprite.Bounds.Position.Y--;
        //        }
        //        else if(trajectory.Direction == Direction.Right)
        //        {
        //            sprite.Bounds.Position.X++;
        //        }
        //        else if (trajectory.Direction == Direction.Down)
        //        {
        //            sprite.Bounds.Position.Y++;
        //        }
        //        else if (trajectory.Direction == Direction.Left)
        //        {
        //            sprite.Bounds.Position.X--;
        //        }
        //        if(trajectory.EdgeOfScreenCondition == EdgeScreenHandling.Disappear)
        //            HandleScreenEdgeDisappear(sprite, trajectory);
        //        else if (trajectory.EdgeOfScreenCondition == EdgeScreenHandling.ReverseDirection)
        //            HandleScreenEdgeReverseDirection(sprite, trajectory);
        //    }
        //}
    }
}
