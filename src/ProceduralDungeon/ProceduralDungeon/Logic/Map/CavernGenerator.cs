using System;

namespace ProceduralDungeon
{
    public class CavernGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Random _random;
        private readonly int _fillPercent;
        private readonly int _iterations;

        public CavernGenerator(int width, int height, Random random, int fillPercent = 45, int iterations = 5)
        {
            _width = width;
            _height = height;
            _random = random;
            _fillPercent = fillPercent;
            _iterations = iterations;
        }

        public TileType[,] GenerateCavern()
        {
            var map = new TileType[_width, _height];
            // Инициализация случайным образом
            for (var x = 0; x < _width; x++)
            for (var y = 0; y < _height; y++)
            {
                if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
                    map[x, y] = TileType.Wall;
                else
                    map[x, y] = (_random.Next(100) < _fillPercent) ? TileType.Wall : TileType.Floor;
            }

            // Итерации клеточного автомата
            for (var i = 0; i < _iterations; i++)
                map = DoSimulationStep(map);

            return map;
        }

        private TileType[,] DoSimulationStep(TileType[,] oldMap)
        {
            var newMap = new TileType[_width, _height];
            for (var x = 0; x < _width; x++)
            for (var y = 0; y < _height; y++)
            {
                var wallCount = GetAdjacentWallCount(oldMap, x, y);
                if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
                    newMap[x, y] = TileType.Wall;
                else if (wallCount > 4)
                    newMap[x, y] = TileType.Wall;
                else if (wallCount < 4)
                    newMap[x, y] = TileType.Floor;
                else
                    newMap[x, y] = oldMap[x, y];
            }
            return newMap;
        }

        private int GetAdjacentWallCount(TileType[,] map, int x, int y)
        {
            var count = 0;
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (nx < 0 || ny < 0 || nx >= _width || ny >= _height)
                    count++;
                else if (map[nx, ny] == TileType.Wall)
                    count++;
            }
            return count;
        }
    }
} 