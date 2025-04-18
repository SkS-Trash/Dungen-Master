using UnityEngine;
using Weapon;

[RequireComponent(typeof(Collider))]
public class HitPoint : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private UnitType[] targetUnits;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<WeaponMarker>(out var weaponMarker)) return;

        foreach (var target in targetUnits)
        {
            if (weaponMarker.UnitType != target) continue;

            var damage = weaponMarker.Damage;
            health.TakeDamage((int)damage);
        }
    }
}