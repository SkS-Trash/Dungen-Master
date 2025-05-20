using System;
using UnityEngine;

namespace Progress
{
    [Serializable]
    public class EnemyData
    {
        public string guid;
        [Space] public int health;
        [Space] public Vector3Serializable position;
        public Vector3Serializable rotation;
    }
}