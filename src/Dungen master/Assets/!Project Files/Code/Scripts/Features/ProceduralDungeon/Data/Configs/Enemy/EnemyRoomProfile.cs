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

        [field: SerializeField, HideLabel, Title("Типы врагов", titleAlignment: TitleAlignments.Centered),
                ListDrawerSettings, InfoBox("Список типов врагов, которые могут появиться в комнате")]
        public EnemyType[] EnemyTypes { get; private set; } = Array.Empty<EnemyType>();

        [field: SerializeField, HideLabel, Title("Вес врагов", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Вес врагов")]
        public SerializableDictionary<EnemyType, int> EnemyWeights { get; private set; } = new();

        [field: SerializeField, HideLabel, Title("Конфигурация врагов", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Конфигурация врагов")]
        public SerializableDictionary<EnemyType, EnemyConfig[]> EnemyConfigs { get; private set; } = new();

        public void Validate()
        {
            RemoveDuplicateEnemyTypes();
            EnsureConfigsContainEnemyTypes();
            EnsureWeightsContainEnemyTypes();
            RemoveInvalidWeights();
            RemoveInvalidConfigs();
            MinWeight();
        }

        private void RemoveDuplicateEnemyTypes()
        {
            EnemyTypes = EnemyTypes.Distinct().ToArray();
        }

        private void EnsureConfigsContainEnemyTypes()
        {
            foreach (var enemyType in EnemyTypes)
            {
                EnemyConfigs.TryAdd(enemyType, null);
            }
        }

        private void EnsureWeightsContainEnemyTypes()
        {
            foreach (var enemyType in EnemyTypes)
            {
                EnemyWeights.TryAdd(enemyType, 1);
            }
        }

        private void RemoveInvalidWeights()
        {
            var invalidKeys = EnemyWeights
                .Keys
                .Where(enemyType => !EnemyTypes.Contains(enemyType))
                .ToList();

            foreach (var key in invalidKeys)
            {
                EnemyWeights.Remove(key);
            }
        }

        private void RemoveInvalidConfigs()
        {
            var invalidKeys = EnemyConfigs
                .Keys
                .Where(enemyType => !EnemyTypes.Contains(enemyType))
                .ToList();

            foreach (var key in invalidKeys)
            {
                EnemyConfigs.Remove(key);
            }
        }

        private void MinWeight()
        {
            foreach (var key in EnemyWeights.Keys.Where(key => EnemyWeights[key] < 1))
            {
                EnemyWeights[key] = 1;
            }
        }
    }
}