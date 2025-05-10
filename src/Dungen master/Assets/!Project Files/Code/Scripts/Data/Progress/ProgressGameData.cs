using System;
using System.Collections.Generic;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Configs.Enemy;
using ProceduralDungeon.Data.Configs.Map;
using UnityEngine;

namespace Progress
{
    [Serializable]
    public class LevelSaveData
    {
        public int currentLevelIndex = -1;

        [Space] public DungeonLevelData dungeon = new();
        [Space] public PlayerLevelData player = new();
        [Space] public List<EnemyData> enemies = new();
        [Space] public List<DynamicEntityData> dynamicEntities = new();
    }

    [Serializable]
    public class DungeonLevelData
    {
        public int seed;
        [Space] public int styleIndex;
        [Space] public TileGeneratorConfig mapConfig;
        [Space] public DecorGeneratorConfig decorConfig;
        [Space] public EnemyGeneratorConfig enemyConfig;
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
}