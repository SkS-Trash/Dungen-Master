using Cysharp.Threading.Tasks;
using Enemy.Components.UI;
using Enemy.Core;
using Factories.GameObject;
using Health;
using Player.Components;
using Providers.Assets;
using R3;
using Services.AudioPlayback;
using StateMachines.DirectControlMultiLayer;
using UnityEngine;
using AudioType = Services.AudioPlayback.AudioType;

namespace Core.Project.Dungeon
{
    public class LaunchBossDungeon : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IAudioPlaybackService _audioPlayback;
        private readonly IAssetsProvider _assetsProvider;

        public LaunchBossDungeon(
            IGameObjectFactory gameObjectFactory,
            IAudioPlaybackService audioPlayback,
            IAssetsProvider assetsProvider
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _audioPlayback = audioPlayback;
            _assetsProvider = assetsProvider;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            await InstantBoss();

            await StartSoundtrack();
        }

        private async UniTask InstantBoss()
        {
            var player = Object.FindFirstObjectByType<ThirdPersonController>().transform;

            var enemyInstance = await _gameObjectFactory.InstantiateAsync($"Enemy/Boss/{Random.Range(1, 2)}",
                new Vector3(player.position.x, 0, player.position.z));
            enemyInstance.name = $"Enemy - Boss)";

            var stateController = enemyInstance.GetComponent<StateController>();
            var stateDraw = enemyInstance.GetComponentInChildren<EnemyCurrentStateDraw>();
            stateController.Stats.Subscribe(
                state => stateDraw.SetStateText(state.ToString()),
                state => stateDraw.SetStateText(state.ToString())
            );

            var healthContainer = enemyInstance.GetComponent<HealthContainer>();
            var healthBar = enemyInstance.GetComponentInChildren<HealthBar>();
            healthContainer.HealthPercentage.Subscribe(healthBar.SetHealthPercentage);

            stateController.SetPlayerTransform(player);
        }


        private async UniTask StartSoundtrack()
        {
            var soundtrack = await _assetsProvider.GetAsset<AudioClip>(SoundsPaths.GameBossSoundtrack);
            _audioPlayback.PlayAudio(soundtrack, AudioType.Music, true);
        }
    }
}