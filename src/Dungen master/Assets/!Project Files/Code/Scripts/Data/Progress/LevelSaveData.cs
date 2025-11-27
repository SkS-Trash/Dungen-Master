using System;
using System.Collections.Generic;
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