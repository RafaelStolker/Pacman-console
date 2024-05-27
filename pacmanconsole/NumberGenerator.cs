namespace Pacman_console
{
	public class NumberGenerator
	{
		static Random _random = new();

		public static int Generate(int min, int max)
		{
			return _random.Next(min, max); ;

		}
	}
}
