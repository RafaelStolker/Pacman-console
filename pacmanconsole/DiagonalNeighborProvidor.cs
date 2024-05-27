﻿using AStarNavigator;
using AStarNavigator.Providers;

namespace Pacman_console
{
	public class DiagonalNeighborProvider : INeighborProvider
	{
        private static readonly double[,] neighbors = new double[,]
        {
            { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }
        };

        public IEnumerable<Tile> GetNeighbors(Tile tile)
        {
            var result = new List<Tile>();

            for (var i = 0; i < neighbors.GetLongLength(0); i++)
            {
                result.Add(new Tile(
                    x: tile.X + neighbors[i, 0],
                    y: tile.Y + neighbors[i, 1]
                ));
            }

            return result;
        }
    }
}

