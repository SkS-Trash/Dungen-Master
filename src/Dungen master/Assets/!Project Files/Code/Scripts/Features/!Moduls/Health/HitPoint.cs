using UnityEngine;
using Weapon;

namespace Health
{
    [RequireComponent(typeof(Collider))]
    public class HitPoint : MonoBehaviour, ITakeDamage
    {
        [field: SerializeField] public UnitType Owner { get; private set; } = UnitType.Enemy;

        [SerializeField] private HealthContainer health;

        public void TakeHit(float amount)
        {
            health.TakeDamage((int)amount);
        }
    }
}