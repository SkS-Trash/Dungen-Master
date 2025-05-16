using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Data/Items/" + nameof(WeaponConfig))]
    public class WeaponConfig : ItemConfig
    {
        [field: SerializeField] public float Damage { get; private set; } = 10;
        [field: Space]
        [field: SerializeField] public Vector3 OffsetRotation { get; private set; } = Vector3.zero;
        [field: SerializeField] public Vector3 OffsetPosition { get; private set; } = Vector3.zero;
    }
}