using System;
using System.Collections.Generic;
using Dungeon;

namespace Features.ProceduralDungeon
{
    public class EnemySpawner
    {
        public readonly EnemyType[,] EnemyLayer;
        private readonly Random _random = new();

        public EnemySpawner(int width, int height)
        {
            EnemyLayer = new EnemyType[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    EnemyLayer[x, y] = EnemyType.None;
                }
            }
        }

        public void SpawnEnemies(TileType[,] map, List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                // Случайное количество врагов (0..2) в каждой комнате
                var enemyCount = _random.Next(0, 3);
                for (var i = 0; i < enemyCount; i++)
                {
                    var x = _random.Next(room.X + 1, room.X + room.Width - 1);
                    var y = _random.Next(room.Y + 1, room.Y + room.Height - 1);

                    // 50% шанс ближнего боя, 50% – дальнего
                    var enemy = (_random.NextDouble() < 0.5)
                        ? EnemyType.EnemyIsCloseCombat
                        : EnemyType.EnemyRangedCombat;

                    EnemyLayer[x, y] = enemy;
                }
            }
        }
    }
}