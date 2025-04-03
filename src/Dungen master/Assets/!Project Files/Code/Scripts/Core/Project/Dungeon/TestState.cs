using Cysharp.Threading.Tasks;
using ProceduralDungeon;
using Providers.Data;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer.ForState;
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

            var levelStyleConfigs = _staticDataProvider.GetLevelStyleConfigs();
            var styleConfig = levelStyleConfigs[Random.Range(0, levelStyleConfigs.Length)];
            await _projectEngine.RunOneShot<GenerateMapState, DungeonGenerationData>(data);
            await _projectEngine.RunOneShot<ConstructionMapState, (TileType[,], LevelStyleConfig)>((data.MapLayer,
                styleConfig));

            await _projectEngine.RunOneShot<ConstructionDecorState, (DecorType[,], LevelStyleConfig)>((data.DecorLayer,
                styleConfig));

            await _projectEngine.RunOneShot<BakeNavMeshState>();

            await _projectEngine.RunOneShot<ConstructionEnemyState, (EnemyType[,], LevelStyleConfig)>((data.EnemyLayer,
                styleConfig));

            await _projectEngine.RunOneShot<InstantiateUIState>();

            await _projectEngine.RunOneShot<InstantiatePlayerState, TileType[,]>(data.MapLayer);

            await _projectEngine.RunOneShot<ConfiguredGameState>();
        }
    }
}