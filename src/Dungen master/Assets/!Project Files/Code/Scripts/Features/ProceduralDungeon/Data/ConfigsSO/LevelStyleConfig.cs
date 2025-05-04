using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralDungeon.Data.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data
{
    [CreateAssetMenu(fileName = "LevelStyleConfig", menuName = "Data/Dungeon/LevelStyleConfig")]
    public class LevelStyleConfig : ScriptableObject
    {
        [Header("Tile Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Type",
            ShowPaging = false, ShowItemCount = false, IsReadOnly = true)]
        private List<CellConfig<TileType>> tileConfigs;

        [Header("Enemy Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Type",
            ShowPaging = false, ShowItemCount = false, DraggableItems = false,
            HideRemoveButton = true, HideAddButton = true)]
        private List<CellConfig<EnemyType>> enemyConfigs;

        [Header("Decor Configs"), SerializeField]
        [ListDrawerSettings(ShowFoldout = false, ShowIndexLabels = true, ListElementLabelName = "Type",
            ShowPaging = false, ShowItemCount = false, DraggableItems = false,
            HideRemoveButton = true, HideAddButton = true)]
        private List<CellConfig<DecorType>> decorConfigs;

        public CellConfig<TileType> GetTileConfig(TileType type) =>
            tileConfigs.FirstOrDefault(c => c.Type == type);

        public CellConfig<EnemyType> GetEnemyConfig(EnemyType type) =>
            enemyConfigs.FirstOrDefault(c => c.Type == type);

        public CellConfig<DecorType> GetDecorConfig(DecorType type) =>
            decorConfigs.FirstOrDefault(c => c.Type == type);

#if UNITY_EDITOR
        private void OnValidate() => Validate();

        [Button]
        private void Validate()
        {
            ValidateTileConfigs();
            ValidateEnemyConfigs();
            ValidateDecorConfigs();
        }

        private void ValidateTileConfigs()
        {
            var tileTypes = Enum.GetValues(typeof(TileType)).Cast<TileType>().ToList();

            // Добавить все отсутствующие типы
            var missingTypes = tileTypes.Where(t => tileConfigs.All(c => c.Type != t)).ToList();
            if (missingTypes.Count > 0)
            {
                foreach (var type in missingTypes)
                {
                    tileConfigs.Add(new CellConfig<TileType>(type));
                }
            }

            // Удалить все Empty
            tileConfigs.RemoveAll(c => c.Type == TileType.Empty);
        }

        private void ValidateEnemyConfigs()
        {
            var types = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().ToList();

            // Добавить все отсутствующие типы
            var missingTypes = types.Where(t => enemyConfigs.All(c => c.Type != t)).ToList();
            if (missingTypes.Count > 0)
            {
                foreach (var type in missingTypes)
                {
                    enemyConfigs.Add(new CellConfig<EnemyType>(type));
                }
            }

            // Удалить все None
            enemyConfigs.RemoveAll(c => c.Type == EnemyType.None);
        }

        private void ValidateDecorConfigs()
        {
            var types = Enum.GetValues(typeof(DecorType)).Cast<DecorType>().ToList();

            // Добавить все отсутствующие типы
            var missingTypes = types.Where(t => decorConfigs.All(c => c.Type != t)).ToList();
            if (missingTypes.Count > 0)
            {
                foreach (var type in missingTypes)
                {
                    decorConfigs.Add(new CellConfig<DecorType>(type));
                }
            }

            // Удалить все None
            decorConfigs.RemoveAll(c => c.Type == DecorType.None);
        }

        [Button]
        private void Sort()
        {
            tileConfigs = tileConfigs.OrderBy(c => c.Type).ToList();
            enemyConfigs = enemyConfigs.OrderBy(c => c.Type).ToList();
            decorConfigs = decorConfigs.OrderBy(c => c.Type).ToList();
        }
#endif
    }
}