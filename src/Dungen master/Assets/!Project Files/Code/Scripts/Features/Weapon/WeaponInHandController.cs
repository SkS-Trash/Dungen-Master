using Items;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Weapon
{
    public class WeaponInHandController : MonoBehaviour
    {
        private bool WeaponInHand => isInHand;
        private bool NotInHand => isInHand == false;

        [SerializeField] private bool isInHand;
        [SerializeField] private Transform weaponParent;

        [Space] [SerializeField, HideIf(nameof(WeaponInHand))]
        private WeaponConfig startingWeapon;

        [SerializeField, HideIf(nameof(NotInHand))]
        private Transform weaponTransform;
        
        private WeaponMarker _currentWeapon;

        private void Start()
        {
            if (isInHand)
            {
                EquipWeapon(weaponTransform);
            }
            else if (startingWeapon)
            {
                EquipWeapon(startingWeapon);
            }
        }

        public bool HasWeapon() =>
            _currentWeapon != null;
        
        public void EquipWeapon(Transform weapon)
        {
            UnequipCurrentWeapon();

            if (weapon == null)
            {
                Debug.LogError("Не удалось загрузить оружие!");
                return;
            }

            weapon.SetParent(weaponParent);
            weapon.localPosition = Vector3.zero;
            weapon.localRotation = Quaternion.identity;

            _currentWeapon = weapon.GetComponentInChildren<WeaponMarker>();
            if (_currentWeapon == null)
            {
                Debug.LogError("Загруженное оружие не содержит компонент WeaponMarker!");
                Destroy(weapon.gameObject);
                return;
            }
        }

        public async void EquipWeapon(WeaponConfig config)
        {
            UnequipCurrentWeapon();

            var weaponInstance = await Addressables.InstantiateAsync(config.ObjectReference, weaponParent).Task;
            if (weaponInstance == null)
            {
                Debug.LogError("Не удалось загрузить оружие!");
                return;
            }

            weaponInstance.transform.localPosition = config.OffsetPosition;
            weaponInstance.transform.localRotation = Quaternion.Euler(config.OffsetRotation);

            _currentWeapon = weaponInstance.GetComponentInChildren<WeaponMarker>();
            if (_currentWeapon == null)
            {
                Debug.LogError("Загруженное оружие не содержит компонент WeaponMarker!");
                Destroy(weaponInstance);
                return;
            }

            _currentWeapon.SetDamage(config.Damage);
        }

        private void UnequipCurrentWeapon()
        {
            if (HasWeapon())
            {
                Destroy(_currentWeapon.gameObject);
            }
        }

        public void EnableWeaponCollider()
        {
            if (!HasWeapon())
            {
                Debug.LogWarning("Нет оружия в руке!");
                return;
            }

            _currentWeapon.EnableWeaponColliders();
        }

        public void DisableWeaponCollider()
        {
            if (!HasWeapon())
            {
                Debug.LogWarning("Нет оружия в руке!");
                return;
            }

            _currentWeapon.DisableWeaponColliders();
        }
    }
}