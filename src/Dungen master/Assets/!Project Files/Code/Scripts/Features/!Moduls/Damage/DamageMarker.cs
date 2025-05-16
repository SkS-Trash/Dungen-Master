using System.Linq;
using UnityEngine;

namespace Damage
{
    [RequireComponent(typeof(Collider))]
    public class DamageMarker : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; protected set; }
        [field: SerializeField] public UnitType[] TargetUnits { get; protected set; } = { UnitType.Player };

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        public void SetTargetUnits(UnitType[] targetUnits)
        {
            TargetUnits = targetUnits;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITakeDamage damageable) &&
                TargetUnits.Any(unitType => unitType == damageable.Owner))
            {
                damageable.TakeHit(Damage);
            }
        }
    }
}