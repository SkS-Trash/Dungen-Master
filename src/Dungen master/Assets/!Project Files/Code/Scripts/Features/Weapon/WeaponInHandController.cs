using Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Weapon
{
    public class WeaponInHandController : MonoBehaviour
    {
        [SerializeField] private WeaponConfig startingWeapon;

        [SerializeField] private Transform weaponParent;

        private WeaponMarker _currentWeapon;

        private void Start()
        {
            if (startingWeapon)
                EquipWeapon(startingWeapon);
        }

        public bool HasWeapon() => _currentWeapon != null;

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
            if (_currentWeapon != null)
            {
                Destroy(_currentWeapon.gameObject);
            }
        }

        public void EnableWeaponCollider()
        {
            if (_currentWeapon == null)
            {
                Debug.LogWarning("Нет оружия в руке!");
                return;
            }

            _currentWeapon.EnableWeaponColliders();
        }

        public void DisableWeaponCollider()
        {
            if (_currentWeapon == null)
            {
                Debug.LogWarning("Нет оружия в руке!");
                return;
            }

            _currentWeapon.DisableWeaponColliders();
        }
    }
}