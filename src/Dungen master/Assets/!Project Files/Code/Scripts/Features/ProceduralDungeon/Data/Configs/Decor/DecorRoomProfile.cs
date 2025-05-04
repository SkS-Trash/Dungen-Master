using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs.Decor
{
    [Serializable]
    public class DecorRoomProfile
    {
        [field: SerializeField, HideLabel, Title("Базовая плотность декора", titleAlignment: TitleAlignments.Centered),
                Range(0, 5), InfoBox("Плотность декора в комнате от 0 до 5, " +
                                     "где 0 - нет декора, 5 - максимальная плотность (заполнено 50% комнаты)")]
        public int BaseDensity { get; private set; } = 1;

        [field: SerializeField, LabelText(" "), Title("Специальные объекты", titleAlignment: TitleAlignments.Centered),
                ListDrawerSettings, InfoBox("Список специальных объектов, которые могут появиться в комнате")]
        public List<DecorType> SpecialObjects { get; private set; } = new();

        [field: SerializeField, HideLabel, Title("Вес декора", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Вес декора")]
        public SerializableDictionary<DecorType, int> DecorWeights { get; private set; } = new();

        [field: SerializeField, HideLabel, Title("Конфигурация декора", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Конфигурация декора")]
        public SerializableDictionary<DecorType, DecorConfig> DecorConfigs { get; private set; } = new();

        public void Validate()
        {
            RemoveDuplicateSpecialObjects();
            EnsureConfigsContainSpecialObjects();
            EnsureWeightsContainSpecialObjects();
            RemoveInvalidWeights();
            RemoveInvalidConfigs();
            MinWeight();
        }

        private void EnsureConfigsContainSpecialObjects()
        {
            foreach (var decorType in SpecialObjects)
            {
                DecorConfigs.TryAdd(decorType, null);
            }
        }

        private void EnsureWeightsContainSpecialObjects()
        {
            foreach (var decorType in SpecialObjects)
            {
                DecorWeights.TryAdd(decorType, 1);
            }
        }

        private void RemoveInvalidWeights()
        {
            var invalidKeys = DecorWeights
                .Keys
                .Where(decorType => !SpecialObjects.Contains(decorType))
                .ToList();

            foreach (var key in invalidKeys)
            {
                DecorWeights.Remove(key);
            }
        }

        private void RemoveInvalidConfigs()
        {
            var invalidKeys = DecorConfigs
                .Keys
                .Where(decorType => !SpecialObjects.Contains(decorType))
                .ToList();

            foreach (var key in invalidKeys)
            {
                DecorConfigs.Remove(key);
            }
        }

        private void MinWeight()
        {
            foreach (var key in DecorWeights.Keys.Where(key => DecorWeights[key] < 1))
            {
                DecorWeights[key] = 1;
            }
        }

        private void RemoveDuplicateSpecialObjects()
        {
            SpecialObjects = SpecialObjects.Distinct().ToList();
        }
    }
}