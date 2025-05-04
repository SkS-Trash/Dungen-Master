using ProceduralDungeon.Data.Configs.Decor;
using ProceduralDungeon.Data.Configs.Enemy;
using ProceduralDungeon.Data.Configs.Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralDungeon.Data.Configs
{
    [CreateAssetMenu(fileName = nameof(BaseGeneratorConfig), menuName = "Data/Dungeon/" + nameof(BaseGeneratorConfig))]
    public class BaseGeneratorConfig : ScriptableObject
    {
        [field: SerializeField, HideLabel, BoxGroup(nameof(MapGenerator), centerLabel: true)]
        public MapGeneratorConfig MapGenerator { get; private set; } = new();

        [field: SerializeField, HideLabel, BoxGroup(nameof(Decor), centerLabel: true)]
        public DecorGeneratorConfig Decor { get; private set; } = new();

        [field: SerializeField, HideLabel, BoxGroup(nameof(Enemy), centerLabel: true)]
        public EnemyGeneratorConfig Enemy { get; private set; } = new();

        [Button("Validate"), GUIColor(0.5f, 0.8f, 0.5f)]
        private void OnValidate()
        {
            Decor.Validate();
            Enemy.Validate();
        }
    }
}