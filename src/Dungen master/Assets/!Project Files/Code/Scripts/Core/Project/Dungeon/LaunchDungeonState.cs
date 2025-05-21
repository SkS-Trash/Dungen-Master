using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Providers.Assets;
using Providers.Containers.Game;
using Providers.Data;
using Services.AudioPlayback;
using Services.Progress;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using AudioType = Services.AudioPlayback.AudioType;

namespace Core.Project.Dungeon
{
    public class LaunchDungeonState : IState, IEnterable
    {
        private readonly IProjectEngine _projectEngine;
        private readonly IStaticDataProvider _staticData;
        private readonly IGameContainerProvider _gameContainer;
        private readonly IProgressService _progress;
        private readonly IAudioPlaybackService _audioPlayback;
        private readonly IAssetsProvider _assetsProvider;

        public LaunchDungeonState(
            IProjectEngine projectEngine,
            IStaticDataProvider staticData,
            IGameContainerProvider gameContainer,
            IProgressService progress,
            IAudioPlaybackService audioPlayback,
            IAssetsProvider assetsProvider
        )
        {
            _projectEngine = projectEngine;
            _staticData = staticData;
            _gameContainer = gameContainer;
            _progress = progress;
            _audioPlayback = audioPlayback;
            _assetsProvider = assetsProvider;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await _projectEngine.RunOneShot<LoadEmptySceneState>();

            ConfigureDungeon();

            await _projectEngine.RunOneShot<GenerateMapState>();
            await _projectEngine.RunOneShot<ConstructionMapState>();
            await _projectEngine.RunOneShot<ConstructionDecorState>();
            await _projectEngine.RunOneShot<BakeNavMeshState>();
            await _projectEngine.RunOneShot<InstantiatePlayerState>();
            await _projectEngine.RunOneShot<ConstructionEnemyState>();
            await _projectEngine.RunOneShot<ConfiguredGameState>();
            await _projectEngine.RunOneShot<SetupGameEventState>();

            await StartSoundtrack();

            TrySaveOrLoadConfig();
        }

        private void ConfigureDungeon()
        {
            var levelStyleConfigs = _staticData.GetLevelStyleConfigs();
            var gameProgress = _progress.GlobalProgress;
            var levelProgress = _progress.LevelProgress;

            if (levelProgress.currentLevelIndex != gameProgress.currentLevelIndex)
            {
                levelProgress.dungeon.seed = Random.Range(0, 10000);
                levelProgress.dungeon.styleIndex = Random.Range(0, levelStyleConfigs.Length);
            }

            var generatorConfig = _staticData.GetBaseGeneratorConfig();
            var container = _gameContainer.Container;
            container.MapGeneratorConfig = generatorConfig.Tile;
            container.DecorConfig = generatorConfig.Decor;
            container.EnemyConfig = generatorConfig.Enemy;
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
                EventBus.RaiseEvent<ILevelProgressLoadEvent>(x => x.OnProgressLoaded(levelProgress));
                EventBus.RaiseEvent<IGlobalProgressLoadEvent>(x => x.OnProgressLoaded(gameProgress));
            }
        }

        private async UniTask StartSoundtrack()
        {
            var soundtrack = await _assetsProvider.GetAsset<AudioClip>(SoundsPaths.GameplaySoundtrack);
            _audioPlayback.PlayAudio(soundtrack, AudioType.Music, true);
        }
    }
}