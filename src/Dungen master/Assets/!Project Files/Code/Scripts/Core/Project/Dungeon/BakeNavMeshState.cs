using Cysharp.Threading.Tasks;
using Factories.GameObject;
using StateMachines.DirectControlMultiLayer.ForState;
using Unity.AI.Navigation;
using UnityEngine;

namespace Core.Project.Dungeon
{
    public class BakeNavMeshState : IStateOneShot
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public BakeNavMeshState(
            IGameObjectFactory gameObjectFactory
        )
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public async UniTask OnEnterAsync(Unit _)
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "NavMeshBake";
            var navMeshSurface = parentObject.AddComponent<NavMeshSurface>();

            navMeshSurface.BuildNavMesh();
        }
    }
}