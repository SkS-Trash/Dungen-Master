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
}