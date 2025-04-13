using UnityEngine;

namespace Weapon
{
    public class WeaponMarker : MonoBehaviour
    {
        public float Damage { get; private set; }

        [SerializeField] private Collider hitDetector;

        private void Awake()
        {
            DisableWeaponColliders();
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }

        public void EnableWeaponColliders()
        {
            if (hitDetector)
                hitDetector.enabled = true;
        }

        public void DisableWeaponColliders()
        {
            if (hitDetector)
                hitDetector.enabled = false;
        }
    }
}