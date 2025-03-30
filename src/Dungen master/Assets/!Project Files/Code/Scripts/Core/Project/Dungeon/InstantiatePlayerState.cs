using Cysharp.Threading.Tasks;
using Factories.GameObject;
using StateMachines.DirectControlMultiLayer.ForState;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class InstantiatePlayerState : IStateOneShot<Vector2Int>
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public InstantiatePlayerState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync(Vector2Int spawnPoint)
        {
            await InstantiatePlayerUI(spawnPoint);
        }

        private async UniTask InstantiatePlayerUI(Vector2Int spawnPoint)
        {
            await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.PLAYER,
                new Vector3(spawnPoint.x, 0, spawnPoint.y), Quaternion.identity);
        }
    }
}