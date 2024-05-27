namespace Pacman_console.Models
{
    public abstract class GameCharacter
    {
        protected String _name;
        protected char _character;
        protected int _xposition;
        protected int _yposition;

        public GameCharacter(String name, int xposition, int yposition, char character)
        {
            _name = name;
            _xposition = xposition;
            _yposition = yposition;
            _character = character;
        }

        public int Xposition { get => _xposition; set => _xposition = value; }
        public int Yposition { get => _yposition; set => _yposition = value; }

        public virtual void Move(int xdiff, int ydiff)
        {
            _xposition += xdiff;
            _yposition += ydiff;
        }

        public virtual void Draw()
        {
            Console.SetCursorPosition(_xposition, _yposition + 1);
            Console.Write(_character);
        }

        public abstract void Die();
    }
}
