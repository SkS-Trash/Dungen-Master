using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProceduralDungeon.Data.Configs.Decor
{
    [CreateAssetMenu(fileName = nameof(DecorConfig), menuName = "Data/Dungeon/" + nameof(DecorConfig))]
    public class DecorConfig : ScriptableObject
    {
        [field: SerializeField, LabelText("Сылка на префаб декора"), Required,
                InfoBox("Ссылка на префаб декора, который будет использоваться в комнате")]
        public AssetReference Reference { get; private set; }

        [field: SerializeField, LabelText("Варианты декора"),
                InfoBox("Список вариантов декора, которые могут появиться в комнате")]
        public Vector3[] AllowedRotations { get; private set; }

        [field: SerializeField, LabelText("Минимальный размер декора")]
        public Vector3 MinScale { get; private set; } = Vector3.one;

        [field: SerializeField, LabelText("Максимальный размер декора")]
        public Vector3 MaxScale { get; private set; } = Vector3.one;

        private void OnValidate()
        {
            if (MinScale.x < 0 || MinScale.y < 0 || MinScale.z < 0)
                MinScale = Vector3.one;

            if (MaxScale.x < 0 || MaxScale.y < 0 || MaxScale.z < 0)
                MaxScale = Vector3.one;

            if (MinScale.x > MaxScale.x || MinScale.y > MaxScale.y || MinScale.z > MaxScale.z)
                MinScale = MaxScale;

            if (AllowedRotations == null || AllowedRotations.Length == 0)
                AllowedRotations = new[] { Vector3.zero };

            for (int i = 0; i < AllowedRotations.Length; i++)
            {
                if (AllowedRotations[i].x < 0 || AllowedRotations[i].y < 0 || AllowedRotations[i].z < 0)
                    AllowedRotations[i] = Vector3.zero;
            }

            if (Reference == null)
                Debug.LogError("Reference is null", this);
        }
    }
}