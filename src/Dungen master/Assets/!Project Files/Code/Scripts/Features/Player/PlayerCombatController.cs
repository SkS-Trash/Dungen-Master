using Magic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private InputActionReference physicalAttackAction;
        [SerializeField] private InputActionReference magicAttackAction;
        [Space]
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private WeaponInHandController weaponController;
        [SerializeField] private MagicCastController magicCastController;

        private bool _isAttackInProgress;

        private void OnEnable()
        {
            physicalAttackAction.action.performed += OnPhysicalAttackPerformed;
            magicAttackAction.action.performed += OnMagicAttackPerformed;
        }

        private void OnDisable()
        {
            physicalAttackAction.action.performed -= OnPhysicalAttackPerformed;
            magicAttackAction.action.performed -= OnMagicAttackPerformed;
        }

        private void OnPhysicalAttackPerformed(InputAction.CallbackContext context)
        {
            if (!_isAttackInProgress && weaponController.HasWeapon())
            {
                _isAttackInProgress = true;
                animationController.MeleeAttack();
            }
        }
        
        private void OnMagicAttackPerformed(InputAction.CallbackContext context)
        {
            if (!_isAttackInProgress && magicCastController.CanCast())
            {
                _isAttackInProgress = true;
                animationController.MagicAttack();
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
        
        // Анимационное событие
        public void CastMagicTrackingEvent()
        {
            if (magicCastController.CanCast())
                magicCastController.CastSpell();
        }

        // Анимационное событиеs
        public void AttackEndEvent()
        {
            _isAttackInProgress = false;
        }
    }
}