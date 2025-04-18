using Damage;
using UnityEngine;

namespace Magic
{
    [RequireComponent(typeof(Collider))]
    public class SpellDamageMarker : DamageMarker
    {
        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        public void SetUnitType(UnitType[] targetUnits)
        {
            TargetUnits = targetUnits;
        }
    }
}