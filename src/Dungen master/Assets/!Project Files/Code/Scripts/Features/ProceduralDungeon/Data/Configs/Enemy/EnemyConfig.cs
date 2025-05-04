using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProceduralDungeon.Data.Configs.Enemy
{
    [CreateAssetMenu(fileName = nameof(EnemyConfig), menuName = "Data/Dungeon/" + nameof(EnemyConfig))]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField, LabelText("Сылка на префаб врага"), Required,
                InfoBox("Ссылка на префаб врага, который будет использоваться в комнате")]
        public AssetReference Reference { get; private set; }

        [field: SerializeField, LabelText("Варианты врага"),
                InfoBox("Список вариантов врага, которые могут появиться в комнате")]
        public Vector3[] AllowedRotations { get; private set; }

        [field: SerializeField, LabelText("Минимальный размер врага")]
        public Vector3 MinScale { get; private set; } = Vector3.one;

        [field: SerializeField, LabelText("Максимальный размер врага")]
        public Vector3 MaxScale { get; private set; } = Vector3.one;
    }
}