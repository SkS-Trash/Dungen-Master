using UnityEngine;
using Weapon;

namespace Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyHitPoint : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<WeaponMarker>(out var weaponMarker))
            {
                var damage = weaponMarker.Damage;
                enemyHealth.TakeDamage((int)damage);
            }
        }
    }
}