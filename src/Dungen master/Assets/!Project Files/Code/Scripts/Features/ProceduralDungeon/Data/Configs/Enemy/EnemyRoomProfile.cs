using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Enemy
{
    [Serializable]
    public class EnemyRoomProfile
    {
        [field: SerializeField, Range(0, 5), LabelText("Мин количество врагов")]
        public int MinCount { get; private set; } = 1;

        [field: SerializeField, Range(0, 10), LabelText("Макс количество врагов")]
        public int MaxCount { get; private set; } = 5;

        [field: SerializeField, LabelText(" "), Title("Вес врагов", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Вес")]
        public SerializableDictionary<EnemyType, int> EnemyWeights { get; private set; } = new();

        public void Validate()
        {
            RemoveDuplicate();
            AddAllAnyEnemyTypes();
            RemoveNoneEnemyType();
        }

        private void RemoveDuplicate()
        {
            var enemies = EnemyWeights
                .Where(item => item.Key != EnemyType.None)
                .ToList();

            foreach (var item in enemies)
                EnemyWeights.Remove(item.Key);

            foreach (var item in enemies) 
                EnemyWeights.Add(item.Key, item.Value);
        }

        private void AddAllAnyEnemyTypes()
        {
            var enums = (EnemyType[])Enum.GetValues(typeof(EnemyType));
            foreach (var enemyType in enums)
            {
                if (enemyType == EnemyType.None) continue;
                
                EnemyWeights.TryAdd(enemyType, 0);
            }
        }
        
        private void RemoveNoneEnemyType()
        {
            EnemyWeights.Remove(EnemyType.None);
        }
    }
}