using Core.Project.MainMenu;
using Cysharp.Threading.Tasks;
using Factories.GameEvent;
using GameEventObserver;
using Providers.Assets;
using Score;
using Services.AudioPlayback;
using Services.ProjectManager;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using AudioType = Services.AudioPlayback.AudioType;

namespace Core.Project.Dungeon
{
    public class SetupGameEventState : IStateOneShot
    {
        private readonly IGameEventFactory _gameEventFactory;
        private readonly IProjectEngine _projectEngine;
        private readonly IAudioPlaybackService _audioPlayback;
        private readonly IAssetsProvider _assetsProvider;

        public SetupGameEventState(
            IGameEventFactory gameEventFactory,
            IProjectEngine projectEngine,
            IAudioPlaybackService audioPlayback,
            IAssetsProvider assetsProvider
        )
        {
            _gameEventFactory = gameEventFactory;
            _projectEngine = projectEngine;
            _audioPlayback = audioPlayback;
            _assetsProvider = assetsProvider;
        }

        public UniTask OnEnterAsync(UnitEmpty _)
        {
            SetupStartPauseEvent();
            SetupPlayerDeathEvent();
            SetupBossDeathEvent();
            SetupEnemyDeathEvent();
            InstantiateScoreCounter();

            return UniTask.CompletedTask;
        }

        private void SetupStartPauseEvent()
        {
            var @event = _gameEventFactory.CreateGameEvent(GameEventType.StartPause);
            @event.Register(OnStartPauseEvent);

            void OnStartPauseEvent(GameEventObserverBehaviour gameEventObserverBehaviour)
            {
                _projectEngine.RunOneShot<GamePauseState>();
            }
        }

        private void SetupPlayerDeathEvent()
        {
            var @event = _gameEventFactory.CreateGameEvent(GameEventType.PlayerDied);
            @event.Register(OnPlayerDeathEvent);

            void OnPlayerDeathEvent(GameEventObserverBehaviour gameEventObserverBehaviour)
            {
                _projectEngine.ChangeState<MainMenuState>();
            }
        }

        private void SetupBossDeathEvent()
        {
            var @event = _gameEventFactory.CreateGameEvent(GameEventType.BossDied);
            @event.Register(OnBossDeathEvent);

            void OnBossDeathEvent(GameEventObserverBehaviour gameEventObserverBehaviour)
            {
                _projectEngine.ChangeState<LaunchNextLevelDungeon>();
            }
        }

        private void SetupEnemyDeathEvent()
        {
            var enemyDiedCount = 0;
            const int enemyDeathTargetForLaunchNewLevel = 7;
            const int enemyDeathTargetForSpawnBoss = 3;
            var @event = _gameEventFactory.CreateGameEvent(GameEventType.EnemyDied);
            @event.Register(OnEnemyDeathEvent);

            void OnEnemyDeathEvent(GameEventObserverBehaviour gameEventObserverBehaviour)
            {
                enemyDiedCount++;

                LaunchEnemyDeathSound();

                if (enemyDiedCount == enemyDeathTargetForSpawnBoss)
                {
                    _projectEngine.RunOneShot<LaunchBossDungeon>();
                }

                if (enemyDiedCount == enemyDeathTargetForLaunchNewLevel)
                {
                    _projectEngine.ChangeState<LaunchNextLevelDungeon>();
                }
            }

            async void LaunchEnemyDeathSound()
            {
                var soundtrack = await _assetsProvider.GetAsset<AudioClip>(SoundsPaths.GameEnemyDeathSoundtrack);
                _audioPlayback.PlayAudio(soundtrack, AudioType.SoundEffect);
            }
        }

        private void InstantiateScoreCounter()
        {
            var scoreCounterGO = new GameObject();
            scoreCounterGO.AddComponent<ScoreCounter>();
        }
    }
}