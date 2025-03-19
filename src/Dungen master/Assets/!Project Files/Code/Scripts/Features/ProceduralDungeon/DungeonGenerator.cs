using System.Collections.Generic;
using Dungeon;

namespace Features.ProceduralDungeon
{
    public class DungeonGenerator
    {
        public TileType[,] MapLayer;
        public DecorType[,] DecorLayer;
        public EnemyType[,] EnemyLayer;

        private List<Room> _rooms;

        private readonly int _width;
        private readonly int _height;

        public DungeonGenerator(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void GenerateDungeon(int roomCount, int roomMinSize, int roomMaxSize)
        {
            // Генерация базовой карты
            var mapGen = new MapGenerator(_width, _height);
            mapGen.GenerateMap(roomCount, roomMinSize, roomMaxSize);
            MapLayer = mapGen.Map;
            _rooms = mapGen.Rooms;

            // Генерация декора
            var decorGen = new DecorGenerator(_width, _height);
            decorGen.GenerateDecor(MapLayer, _rooms);
            DecorLayer = decorGen.DecorLayer;

            // Расстановка врагов
            var enemySpawner = new EnemySpawner(_width, _height);
            enemySpawner.SpawnEnemies(MapLayer, _rooms);
            EnemyLayer = enemySpawner.EnemyLayer;
        }
    }
}