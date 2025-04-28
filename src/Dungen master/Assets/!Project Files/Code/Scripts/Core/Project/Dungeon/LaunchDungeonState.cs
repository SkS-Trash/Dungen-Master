using Cysharp.Threading.Tasks;
using Providers.Containers.Game;
using Providers.Data;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using Subscribers.EventBusSystem;
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

            ConfigureDungeon();

            await _projectEngine.RunOneShot<GenerateMapState>();
            await _projectEngine.RunOneShot<ConstructionMapState>();
            await _projectEngine.RunOneShot<ConstructionDecorState>();
            await _projectEngine.RunOneShot<BakeNavMeshState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<InstantiateUIState>();
            await _projectEngine.RunOneShot<ConstructionEnemyState>();
            await _projectEngine.RunOneShot<ConfiguredGameState>();

            TrySaveOrLoadConfig();
        }

        private void ConfigureDungeon()
        {
            var levelStyleConfigs = _staticData.GetLevelStyleConfigs();
            var gameProgress = _progress.GlobalProgress;
            var levelProgress = _progress.LevelProgress;

            if (levelProgress.currentLevelIndex != gameProgress.currentLevelIndex)
            {
                levelProgress.dungeon.width = 50;
                levelProgress.dungeon.height = 50;
                levelProgress.dungeon.roomCount = 10;
                levelProgress.dungeon.roomMinSize = 5;
                levelProgress.dungeon.roomMaxSize = 10;

                levelProgress.dungeon.seed = Random.Range(0, 10000);

                var styleIndex = Random.Range(0, levelStyleConfigs.Length);
                levelProgress.dungeon.styleIndex = styleIndex;
            }

            var container = _gameContainer.Container;
            container.Width = levelProgress.dungeon.width;
            container.Height = levelProgress.dungeon.height;
            container.RoomCount = levelProgress.dungeon.roomCount;
            container.RoomMinSize = levelProgress.dungeon.roomMinSize;
            container.RoomMaxSize = levelProgress.dungeon.roomMaxSize;
            container.Seed = levelProgress.dungeon.seed;
            container.LevelStyleConfig = levelStyleConfigs[levelProgress.dungeon.styleIndex];
        }

        private void TrySaveOrLoadConfig()
        {
            var levelProgress = _progress.LevelProgress;
            var gameProgress = _progress.GlobalProgress;

            var isSave = levelProgress.currentLevelIndex != gameProgress.currentLevelIndex;
            if (isSave)
            {
                levelProgress.currentLevelIndex = gameProgress.currentLevelIndex;
                _progress.SaveLevel();
            }
            else
            {
                EventBus.RaiseEvent<ILevelProgressLoadSubscriber>(x => x.OnProgressLoaded(levelProgress));
                EventBus.RaiseEvent<IGlobalProgressLoadSubscriber>(x => x.OnProgressLoaded(gameProgress));
            }
        }
    }
}