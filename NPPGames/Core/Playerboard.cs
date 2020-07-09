using System;
using System.Text;

namespace NPPGames.Core
{
    public class Playerboard : Sprite
    {
        public Playerboard(string lifeCharacters, int initialLives) : base(null, null, 20, 1)
        {
            _lifeCharacters = lifeCharacters;
            _lives = initialLives;
            RebuildData();
        }
        private int _points = 0;
        string _lifeCharacters = "";
        int _lives = 0;

        private void RebuildData()
        {
            string format = "                 SCORE {0}                                                                           LIVES {1}";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _lives; i++)
            {
                sb.Append(_lifeCharacters);
                sb.Append(" ");
            }
            var result = string.Format(format, String.Format("{0:n0}", _points).PadRight(7, ' '), sb.ToString());
            Bounds.Size.Width = result.Length;
            base.SetData(result);
        }

        public void AddLife()
        {
            ++_lives;
            RebuildData();
        }

        public void RemoveLife()
        {
            --_lives;
            RebuildData();
        }
        public void AddPoints(int points)
        {
            _points += points;
            RebuildData();
        }

        public bool HasLife()
        {
            return _lives > 0;
        }
    }
}
