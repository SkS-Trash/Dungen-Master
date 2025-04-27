using Cysharp.Threading.Tasks;
using Progress;
using Providers.Containers.Game;
using Providers.Data;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class LaunchDungeonState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IStaticDataProvider _staticData;
        private readonly IGameContainerProvider _gameContainer;
        private readonly IProgressService _progress;

        public LaunchDungeonState(
            IProjectEngine projectEngine,
            IStaticDataProvider staticData,
            IGameContainerProvider gameContainer,
            IProgressService progress
        )
        {
            _projectEngine = projectEngine;
            _staticData = staticData;
            _gameContainer = gameContainer;
            _progress = progress;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            await _projectEngine.RunOneShot<LoadEmptySceneState>();

            var container = _gameContainer.Container;

            container.Width = 50;
            container.Height = 50;
            container.RoomCount = 10;
            container.RoomMinSize = 5;
            container.RoomMaxSize = 10;

            var levelStyleConfigs = _staticData.GetLevelStyleConfigs();
            container.LevelStyleConfig = levelStyleConfigs[Random.Range(0, levelStyleConfigs.Length)];

            await _projectEngine.RunOneShot<GenerateMapState>();
            await _projectEngine.RunOneShot<ConstructionMapState>();
            await _projectEngine.RunOneShot<ConstructionDecorState>();
            await _projectEngine.RunOneShot<BakeNavMeshState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<InstantiateUIState>();
            await _projectEngine.RunOneShot<ConstructionEnemyState>();
            await _projectEngine.RunOneShot<ConfiguredGameState>();
            
            var gameProgress = _progress.GlobalProgress;
            gameProgress.isInDungeon = true;
        }
    }
}