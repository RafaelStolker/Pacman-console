using Pacman_console.Exceptions;

namespace Pacman_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;

            Level level = new();

            try
            {
                ConsoleKey key = new();
                while (key != ConsoleKey.Escape)
                {
                    key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.RightArrow:
                            level.Move(1, 0);
                            break;
                        case ConsoleKey.LeftArrow:
                            level.Move(-1, 0);
                            break;
                        case ConsoleKey.UpArrow:
                            level.Move(0, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            level.Move(0, 1);
                            break;
                    }
                }
            }
            catch (GameOverException) {
                Console.Clear();

                Console.WriteLine($"Final score: {level.Score}");
                Console.WriteLine();

                Console.WriteLine($"Press spacebar to restart");

                while (Console.ReadKey().Key != ConsoleKey.Spacebar)
                {
                }

                Main(args);
            }
        }
    }
}