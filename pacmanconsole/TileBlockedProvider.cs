using AStarNavigator;
using AStarNavigator.Providers;

namespace Pacman_console
{
	public class TileBlockedProvider: IBlockedProvider
	{
		private Level _level;

		public TileBlockedProvider(Level level)
		{
			_level = level;
		}

        public bool IsBlocked(Tile coord)
        {
			return _level.TileHasCollision((int)coord.X, (int)coord.Y);

		}
    }
}
