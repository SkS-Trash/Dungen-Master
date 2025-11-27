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
        [InfoBox("Конфиг содержащий в себе все необходимые параметры для генерации карты. " +
                 "Он используется для первоначальной генерации данных")]
        [ShowInInspector, ReadOnly, HideLabel]
        private string info = " ";
        
        [field: SerializeField, HideLabel, BoxGroup(nameof(Tile), CenterLabel = true)]
        public TileGeneratorConfig Tile { get; private set; } = new();

        [field: SerializeField, HideLabel, BoxGroup(nameof(Decor), CenterLabel = true)]
        public DecorGeneratorConfig Decor { get; private set; } = new();

        [field: SerializeField, HideLabel, BoxGroup(nameof(Enemy), CenterLabel = true)]
        public EnemyGeneratorConfig Enemy { get; private set; } = new();

        [Button("Validate"), GUIColor(0.5f, 0.8f, 0.5f)]
        private void OnValidate()
        {
            Tile.Validate();
            Decor.Validate();
            Enemy.Validate();
        }
    }
}