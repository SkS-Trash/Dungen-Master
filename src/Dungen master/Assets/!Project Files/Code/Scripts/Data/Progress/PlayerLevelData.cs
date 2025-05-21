using System;
using UnityEngine;

namespace Progress
{
    [Serializable]
    public class PlayerLevelData
    {
        public Vector3Serializable position;
        public Vector3Serializable rotation;
        [Space] public int health;
    }
}