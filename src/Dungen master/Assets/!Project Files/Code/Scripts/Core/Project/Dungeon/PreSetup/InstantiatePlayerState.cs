using Cysharp.Threading.Tasks;
using Factories.GameObject;
using Health;
using Magic;
using Player.Components;
using Player.UI;
using ProceduralDungeon.Data.Types;
using Providers.Containers.Game;
using R3;
using Services.Window;
using StateMachines.DirectControlMultiLayer;
using UI.HUD;
using UnityEngine;
using static GameObjectsPaths;

namespace Core.Project.Dungeon
{
    public class InstantiatePlayerState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;
        private readonly IGameContainerProvider _gameContainer;
        private readonly IWindowService _windowService;

        private GameObject _player;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory,
            IGameContainerProvider gameContainer,
            IWindowService windowService
        )
        {
            _gameObjectFactory = gameObjectFactory;
            _gameContainer = gameContainer;
            _windowService = windowService;
        }

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            var container = _gameContainer.Container;

            FindAndSetupSpawnPoint(container);

            await InstantiatePlayer(container);

            await InstantiatePlayerUI();
        }

        private void FindAndSetupSpawnPoint(IGameContainer container)
        {
            var mapLayer = container.MapLayer;

            for (var x = 0; x < mapLayer.GetLength(0); x++)
            for (var y = 0; y < mapLayer.GetLength(1); y++)
            {
                if (mapLayer[x, y] != TileType.Start)
                    continue;

                var playerSpawnPoint = new GameObject("PlayerSpawnPoint")
                {
                    transform =
                    {
                        position = new Vector3(x, 0, y)
                    }
                };

                container.PlayerSpawnPoint = playerSpawnPoint.transform;

                return;
            }

            container.PlayerSpawnPoint = null;
        }

        private async UniTask InstantiatePlayer(IGameContainer container)
        {
            var spawnPoint = container.PlayerSpawnPoint.position;

            _player = await _gameObjectFactory.InstantiateAsync(
                PLAYER,
                spawnPoint,
                Quaternion.identity
            );

            container.PlayerTransform = _player.GetComponentInChildren<ThirdPersonController>().transform;
        }

        private async UniTask InstantiatePlayerUI()
        {
            await _windowService.Open(WindowID.HUD);

            var healthBar = _windowService.Get<HealthBar>(WindowID.HUD);
            var healthContainer = _player.GetComponentInChildren<HealthContainer>();
            healthContainer.HealthPercentage.Subscribe(healthBar.SetHealthPercentage);
            healthBar.SetHealthPercentage(healthContainer.HealthPercentage.Value);

            var magicCooldown = _windowService.Get<PlayerMagicCooldown>(WindowID.HUD);
            var magicContainer = _player.GetComponentInChildren<MagicCastController>();
            magicContainer.SpellCooldown.Subscribe(magicCooldown.SetMagicCooldownPercentage);
            magicCooldown.SetMagicCooldownPercentage(0f);
        }
    }
}