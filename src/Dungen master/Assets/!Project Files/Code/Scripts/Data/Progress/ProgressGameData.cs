using System;
using System.Collections.Generic;
using ProceduralDungeon;
using UnityEngine;

namespace Progress
{
    #region Game progress

    [Serializable]
    public class GlobalSaveData
    {
        public const int VERSION = 1;
        
        public uint version = VERSION;
        public bool isFirstLaunch = true;
        public bool isInDungeon;
    }

    #endregion

    #region Level progress

    [Serializable]
    public class LevelSaveData
    {
        public int levelIndex;
        public DungeonLevelData dungeon = new();
        public PlayerLevelData player = new();
        public List<EnemyData> enemies = new();
        public List<DynamicEntityData> dynamicEntities = new();
    }

    [Serializable]
    public class DungeonLevelData
    {
        public int seed;
        [Space] public int width;
        public int height;
        public int roomCount;
        public int roomMinSize;
        public int roomMaxSize;
    }

    [Serializable]
    public class PlayerLevelData
    {
        public Vector3Serializable position;
        public Vector3Serializable rotation;
        [Space] public int health;
    }

    [Serializable]
    public class EnemyData
    {
        public string guid;
        public EnemyType type;
        [Space] public int health;
        [Space] public Vector3Serializable position;
        public Vector3Serializable rotation;
    }

    [Serializable]
    public class DynamicEntityData
    {
        public string guid;
        public string typeId;
        public DynamicSaveData data;
    }

    [Serializable]
    public abstract class DynamicSaveData
    {
    }

    #endregion

    #region Other data

    [Serializable]
    public class Vector3Serializable
    {
        public float x;
        public float y;
        public float z;

        public Vector3Serializable(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public static implicit operator Vector3(Vector3Serializable vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        public static implicit operator Vector3Serializable(Vector3 vector)
        {
            return new Vector3Serializable(vector);
        }
    }

    #endregion
}