using System.Collections.Generic;

namespace ProceduralDungeon.Enemy
{
    public interface IEnemySpawner
    {
        void SpawnEnemies(TileType[,] map, List<Room> rooms);
        EnemyType[,] EnemyLayer { get; }
    }
}