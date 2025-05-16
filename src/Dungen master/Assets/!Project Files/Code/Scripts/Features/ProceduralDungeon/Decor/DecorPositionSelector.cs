using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Types;

namespace ProceduralDungeon
{
    public class DecorPositionSelector
    {
        public (int x, int y, int density)[] FindValidDecorPositions(Room room, TileType[,] map,
            DecorType[,] decorLayer, int minDistance, DecorDistanceChecker distanceChecker, out int count)
        {
            var maxPositions = (room.Width - 2) * (room.Height - 2);
            var validPositions = new (int x, int y, int density)[maxPositions];

            count = 0;

            for (var x = room.X + 1; x < room.X + room.Width - 1; x++)
            for (var y = room.Y + 1; y < room.Y + room.Height - 1; y++)
            {
                if (!IsPositionValid(x, y, map) ||
                    distanceChecker.HasNearbyDecor(x, y, minDistance, decorLayer))
                    continue;

                var density = CalculatePositionDensity(x, y, map, decorLayer);
                validPositions[count++] = (x, y, density);
            }

            return validPositions;
        }

        public void SortPositionsByDensity((int x, int y, int density)[] positions, int count)
        {
            Array.Sort(positions, 0, count,
                Comparer<(int x, int y, int density)>.Create((a, b) => b.density.CompareTo(a.density)));
        }

        private bool IsPositionValid(int x, int y, TileType[,] map)
        {
            return map[x, y] == TileType.Floor;
        }

        private int CalculatePositionDensity(int x, int y, TileType[,] map, DecorType[,] decorLayer)
        {
            var density = 0;
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, -1, 0, 1 };
            for (var dir = 0; dir < 4; dir++)
            {
                var nx = x + dx[dir];
                var ny = y + dy[dir];
                if (nx >= 0 && nx < map.GetLength(0) &&
                    ny >= 0 && ny < map.GetLength(1))
                {
                    if (map[nx, ny] == TileType.Floor &&
                        decorLayer[nx, ny] == DecorType.None)
                        density++;
                }
            }

            return density;
        }
    }
}