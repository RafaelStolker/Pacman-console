using Pacman_console.Exceptions;

namespace Pacman_console.Models
{
    public class Player : GameCharacter
    {
        public int Lives = 3;
        protected int _energizedTurns = 0;

        public Player(String name, int xposition, int yposotion, char character)
            : base(name, xposition, yposotion, character) { }

        public void Energize()
        {
            _energizedTurns = 30;
        }

        public bool IsEnergized()
        {
            return _energizedTurns > 0;
        }

        public override void Move(int xdiff, int ydiff)
        {
            base.Move(xdiff, ydiff);

            _energizedTurns--;
        }

        public override void Draw()
        {
            Console.SetCursorPosition(_xposition, _yposition + 1);
            Console.Write(IsEnergized() ? 'E' : _character);
        }

        public override void Die()
        {
            Lives--;
            Xposition = 15;
            Yposition = 16;

            if (Lives == 0)
            {
                throw new GameOverException();
            }
        }
    }
}
