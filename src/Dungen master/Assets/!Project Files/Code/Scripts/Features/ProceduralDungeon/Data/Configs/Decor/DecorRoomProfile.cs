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
        public float BaseDensity { get; private set; } = 1;

        [field: SerializeField, LabelText(" "), Title("Конфигурация декора", titleAlignment: TitleAlignments.Centered),
                DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true,
                    KeyLabel = "Тип", KeyColumnWidth = 100, ValueLabel = "Вес")]
        public SerializableDictionary<DecorType, int> DecorWeights { get; private set; } = new();

        public void Validate()
        {
            RemoveDuplicate();
            AddAllAnyDecorTypes();
            RemoveNoneDecorType();
        }

        private void RemoveDuplicate()
        {
            var specialObjects = DecorWeights
                .Where(item => item.Key != DecorType.None)
                .ToList();

            foreach (var item in specialObjects)
            {
                DecorWeights.Remove(item.Key);
            }

            foreach (var item in specialObjects)
            {
                DecorWeights.Add(item.Key, item.Value);
            }
        }

        private void AddAllAnyDecorTypes()
        {
            var enums = (DecorType[])Enum.GetValues(typeof(DecorType));
            foreach (var enumValue in enums)
            {
                if (enumValue == DecorType.None) continue;

                DecorWeights.TryAdd(enumValue, 0);
            }
        }

        private void RemoveNoneDecorType()
        {
            DecorWeights.Remove(DecorType.None);
        }
    }
}