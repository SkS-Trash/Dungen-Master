using Cysharp.Threading.Tasks;
using Factories.GameObject;
using StateMachines.DirectControlMultiLayer;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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

        public async UniTask OnEnterAsync(UnitEmpty _)
        {
            var parentObject = await _gameObjectFactory.InstantiateAsync(GameObjectsPaths.EMPTY_GAME_OBJECT);
            parentObject.name = "NavMeshBake";
            
            var navMeshSurface = parentObject.AddComponent<NavMeshSurface>();
            navMeshSurface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
            navMeshSurface.layerMask = LayerMask.GetMask("Ground");
            navMeshSurface.overrideTileSize = true;
            navMeshSurface.tileSize = 256;
            navMeshSurface.overrideVoxelSize = true;
            navMeshSurface.voxelSize = 0.1f;

            navMeshSurface.BuildNavMesh();
        }
    }
}