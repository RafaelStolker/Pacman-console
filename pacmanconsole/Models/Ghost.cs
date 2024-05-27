namespace Pacman_console.Models
{
    public class Ghost : GameCharacter
    {
        public Ghost(String name, int xposition, int yposotion, char character)
            : base(name, xposition, yposotion, character) { }

        public override void Die()
        {
            Xposition = 15;
            Yposition = 8;
        }
    }
}
