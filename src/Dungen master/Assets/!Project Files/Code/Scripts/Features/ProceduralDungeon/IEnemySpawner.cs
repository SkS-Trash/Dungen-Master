using System.Collections.Generic;

namespace ProceduralDungeon
{
    public interface IEnemySpawner
    {
        void SpawnEnemies(TileType[,] map, List<Room> rooms);
        EnemyType[,] EnemyLayer { get; }
    }
}