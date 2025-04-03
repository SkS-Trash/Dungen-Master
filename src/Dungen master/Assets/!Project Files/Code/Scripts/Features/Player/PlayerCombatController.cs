using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private WeaponInHandController weaponController;

        private bool _isAttackInProgress;

        private void OnEnable()
        {
            inputActionReference.action.performed += OnAttackPerformed;
        }

        private void OnDisable()
        {
            inputActionReference.action.performed -= OnAttackPerformed;
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            if (!_isAttackInProgress && weaponController.HasWeapon())
            {
                _isAttackInProgress = true;
                animationController.MeleeAttack();
            }
        }

        // Анимационное событие
        public void EnableMeleeHitTrackingEvent()
        {
            if (weaponController.HasWeapon())
                weaponController.EnableWeaponCollider();
        }

        // Анимационное событие
        public void DisableMeleeHitTrackingEvent()
        {
            if (weaponController.HasWeapon())
                weaponController.DisableWeaponCollider();
        }

        // Анимационное событиеs
        public void AttackEndEvent()
        {
            _isAttackInProgress = false;
        }
    }
}