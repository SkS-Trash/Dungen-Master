using Damage;
using UnityEngine;

namespace Weapon
{
    public class WeaponMarker : DamageMarker
    {
        [SerializeField] private Collider hitDetector;

        private void Awake()
        {
            DisableWeaponColliders();
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