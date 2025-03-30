using System;
using UnityEngine;

namespace Providers.Containers.Scene
{
    [Serializable]
    public class HomeSceneContainer : ISceneContainer,
        IPlayerSpawnData
    {
        [field: SerializeField] public Transform SpawnPoint { get; private set; }
    }
}