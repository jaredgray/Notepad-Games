
namespace NPPGames.Core.Primitives
{
    public class Size
    {
        public Size() { }
        public Size(int width, int height)
        {
            _width = width;
            _height = height;
        }
        public int Width
        {
            get { return _width; }
            set
            {
                if (!_isLocked)
                {
                    _width = value;
                }
            }
        }
        private int _width;
        public int Height 
        {
            get { return _height; }
            set 
            { 
                if(!_isLocked)
                {
                    _height = value;
                }
            }
        }
        private int _height;

        private bool _isLocked;
        public void Lock() 
        {
            _isLocked = true;
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        public bool IsLocked()
        {
            return _isLocked;
        }

    }
}
