using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public interface IEnemySpawner
    {
        void SpawnEnemies(TileType[,] map, DecorType[,] decorLayer, List<Room> rooms);
        EnemyType[,] EnemyLayer { get; }
    }
}