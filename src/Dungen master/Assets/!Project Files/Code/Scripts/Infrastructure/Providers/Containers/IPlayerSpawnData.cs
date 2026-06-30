using UnityEngine;

namespace Providers.Containers
{
    public interface IPlayerSpawnData
    {
        Transform PlayerSpawnPoint { get; set; }
        Transform PlayerTransform { get; set; }
    }
}