using System.Collections.Generic;
using System.IO;
using NPPGames.Core.Primitives;

namespace NPPGames.Core
{
    public class Sprite
    {
        public Sprite(GameData gameData, Scene scene, int width, int height)
        {
            GameData = gameData;
            Scene = scene;
            Bounds = new Bounds()
            {
                Position = new Vector2D()
                {
                    X = 0,
                    Y = 0
                },
                Size = new Size()
                {
                    Width = width,
                    Height = height
                }
            };
            RendererData = new RendererData();
            Type = "";
        }

        public Scene Scene { get; set; }

        public GameData GameData { get; set; }

        public string Type { get; set; }

        public virtual bool MarkDelete { get; set; }

        public virtual Bounds Bounds { get; private set; }

        public int LayerOrder { get; set; }

        protected void SetData(List<char> storage, string data)
        {
            using (StringReader sr = new StringReader(data))
            {
                var line = "";
                while (null != (line = sr.ReadLine()))
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    for (int i = 0; i < line.Length; i++)
                    {
                        storage.Add(line[i]);
                    }
                }
            }

            if (Bounds.Size.Width * Bounds.Size.Height != storage.Count - 1)
            {
                // something is wrong here
            }
        }

        public void SetData(string data)
        {
            Data = new List<char>(Bounds.Size.Width * Bounds.Size.Height);
            SetData(Data, data);
        }

        protected List<char> Data { get; set; }
        public RendererData RendererData { get; set; }

        public virtual char GetCharAt(int index)
        {
            if(index < Data.Count)
            {
                return Data[index];
            }
            else
            {

            }
            return ' ';
        }

        public virtual void SetCharAt(int index, char value)
        {
            Data[index] = value;
        }

        public virtual void Update() { }

        public TrajectoryController TrajectoryController { get; set; }

        public ICollisionController CollisionController { get; set; }

        public virtual bool CollidesWith(Sprite other, out Sprite collided)
        {
            var result = this.Bounds.IntersectsWith(other.Bounds);
            if (result)
                collided = this;
            else
                collided = null;
            return result;
        }

        public virtual void ResumeGame() { }
    }
}
