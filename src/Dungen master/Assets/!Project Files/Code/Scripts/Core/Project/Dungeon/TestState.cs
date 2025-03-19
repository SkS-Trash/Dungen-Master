using Cysharp.Threading.Tasks;
using Dungeon;
using Infrastructure.Providers.Data;
using Infrastructure.Services.ProjectManager;
using Infrastructure.StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class TestState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IStaticDataProvider _staticDataProvider;

        public TestState(
            IProjectEngine projectEngine,
            IStaticDataProvider staticDataProvider
        )
        {
            _projectEngine = projectEngine;
            _staticDataProvider = staticDataProvider;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var data = new DungeonGenerationData
            {
                Width = 50,
                Height = 50,
                RoomCount = 10,
                RoomMinSize = 5,
                RoomMaxSize = 10
            };
            Debug.Log("Созданы данные для генерации карты");

            var levelStyleConfigs = _staticDataProvider.GetLevelStyleConfigs();
            Debug.Log("Получены конфиги уровней");
            
            var styleConfig = levelStyleConfigs[Random.Range(0, levelStyleConfigs.Length)];
            Debug.Log("Выбран случайный конфиг уровня");

            await _projectEngine.RunOneShot<GenerateMapState, DungeonGenerationData>(data);
            Debug.Log("Сгенерирована карта");

            await _projectEngine.RunOneShot<ConstructionMapState, (TileType[,], LevelStyleConfig)>(
                (data.MapLayer, styleConfig));
            Debug.Log("Построен слой карты");

            await _projectEngine.RunOneShot<ConstructionDecorState, (DecorType[,], LevelStyleConfig)>(
                (data.DecorLayer, styleConfig));

            await _projectEngine.RunOneShot<ConstructionEnemyState, (EnemyType[,], LevelStyleConfig)>(
                (data.EnemyLayer, styleConfig));
        }

    }

    public class DungeonGenerationData
    {
        public int Width;
        public int Height;

        public int RoomCount;
        public int RoomMinSize;
        public int RoomMaxSize;

        public TileType[,] MapLayer;
        public DecorType[,] DecorLayer;
        public EnemyType[,] EnemyLayer;
    }
}