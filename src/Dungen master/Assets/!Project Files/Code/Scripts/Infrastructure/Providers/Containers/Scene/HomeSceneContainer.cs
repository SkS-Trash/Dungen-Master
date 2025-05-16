using System;
using UnityEngine;

namespace Providers.Containers.Scene
{
    [Serializable]
    public class HomeSceneContainer : ISceneContainer
    {
        [field: SerializeField] public Transform PlayerSpawnPoint { get; set; }

        public Transform PlayerTransform { get; set; }
    }
}