using Cysharp.Threading.Tasks;
using Providers.Containers.Game;
using Providers.Data;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class TestState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IGameContainerProvider _containerProvider;

        public TestState(
            IProjectEngine projectEngine,
            IStaticDataProvider staticDataProvider,
            IGameContainerProvider containerProvider
        )
        {
            _projectEngine = projectEngine;
            _staticDataProvider = staticDataProvider;
            _containerProvider = containerProvider;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var container = _containerProvider.Container;

            container.Width = 50;
            container.Height = 50;
            container.RoomCount = 10;
            container.RoomMinSize = 5;
            container.RoomMaxSize = 10;

            var levelStyleConfigs = _staticDataProvider.GetLevelStyleConfigs();
            container.LevelStyleConfig = levelStyleConfigs[Random.Range(0, levelStyleConfigs.Length)];

            await _projectEngine.RunOneShot<GenerateMapState>();
            await _projectEngine.RunOneShot<ConstructionMapState>();
            await _projectEngine.RunOneShot<ConstructionDecorState>();
            await _projectEngine.RunOneShot<BakeNavMeshState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<InstantiateUIState>();
            await _projectEngine.RunOneShot<ConstructionEnemyState>();

            await _projectEngine.RunOneShot<ConfiguredGameState>();
        }
    }
}