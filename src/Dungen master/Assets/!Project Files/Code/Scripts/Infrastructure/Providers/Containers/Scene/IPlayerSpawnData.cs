using UnityEngine;

namespace Providers.Containers.Scene
{
    public interface IPlayerSpawnData
    {
        Transform SpawnPoint { get; }
    }
}