using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Configs.Enemy;
using ProceduralDungeon.Data.Configs.Map;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProceduralDungeon.Data.Configs
{
    [CreateAssetMenu(fileName = "LevelStyleConfig", menuName = "Data/Dungeon/LevelStyleConfig")]
    public class LevelStyleConfig : ScriptableObject
    {
        [Header("Tile Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Label", IsReadOnly = true)]
        private List<CellConfig<TileType>> tileConfigs;

        [Header("Enemy Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Label", IsReadOnly = true)]
        private List<CellConfig<EnemyType>> enemyConfigs;

        [Header("Decor Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Label", IsReadOnly = true)]
        private List<CellConfig<DecorType>> decorConfigs;

        public CellConfig<TileType> GetTileConfig(TileType type) =>
            tileConfigs.FirstOrDefault(c => c.Type == type);

        public CellConfig<EnemyType> GetEnemyConfig(EnemyType type) =>
            enemyConfigs.FirstOrDefault(c => c.Type == type);

        public CellConfig<DecorType> GetDecorConfig(DecorType type) =>
            decorConfigs.FirstOrDefault(c => c.Type == type);

#if UNITY_EDITOR
        private void ValidateAll()
        {
            ValidateConfigs<TileType, TileConfig>(tileConfigs, TileType.Empty);
            ValidateConfigs<EnemyType, EnemyConfig>(enemyConfigs, EnemyType.None);
            ValidateConfigs<DecorType, DecorConfig>(decorConfigs, DecorType.None);
        }

        private void ValidateConfigs<TEnum, TCfg>(List<CellConfig<TEnum>> list, TEnum noneValue)
            where TEnum : Enum
            where TCfg : ItemConfig
        {
            // 1) Добавляем отсутствующие
            var allValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            foreach (var v in allValues)
                if (!EqualityComparer<TEnum>.Default.Equals(v, noneValue)
                    && !list.Any(c => c.Type.Equals(v)))
                    list.Add(new CellConfig<TEnum>(v));

            // 2) Удаляем ненужный «None»
            list.RemoveAll(c => c.Type.Equals(noneValue));

            // 3) Проверяем тип вложенных конфигов
            foreach (var cfg in list)
            foreach (var item in cfg.Configs)
                if (item is not TCfg)
                    Debug.LogError($"[{name}] {typeof(TEnum)}:{cfg.Type} содержит левый {item.GetType()}");

            // 4) Сортируем
            list.Sort((a, b) => Comparer<TEnum>.Default.Compare(a.Type, b.Type));
        }

        [Button, PropertyOrder(0)]
        private void Validate() => ValidateAll();

        [Button, PropertyOrder(1)]
        private void SortAll()
        {
            tileConfigs.Sort((a, b) => Comparer<TileType>.Default.Compare(a.Type, b.Type));
            enemyConfigs.Sort((a, b) => Comparer<EnemyType>.Default.Compare(a.Type, b.Type));
            decorConfigs.Sort((a, b) => Comparer<DecorType>.Default.Compare(a.Type, b.Type));

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        [Serializable]
        public class CellConfig<TTile> where TTile : Enum
        {
            public string Label => $"{Type} ({Configs.Length})";

            public CellConfig(TTile type)
            {
                Type = type;
            }

            [field: SerializeField, HideInInspector]
            public TTile Type { get; private set; }

            [field: SerializeField, HideLabel]
            [field: ListDrawerSettings(ShowFoldout = false)]
            public ItemConfig[] Configs { get; private set; }
        }

        [Serializable]
        public abstract class ItemConfig : ScriptableObject
        {
            [field: SerializeField, LabelText("Сылка на префаб"), Required,
                    InfoBox("Ссылка на префаб, который будет использоваться в комнате")]
            public AssetReference Reference { get; private set; }
        }
    }
}