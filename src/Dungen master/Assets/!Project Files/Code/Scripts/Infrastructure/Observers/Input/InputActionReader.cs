using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Observers.Input
{
    [CreateAssetMenu(menuName = "Data/Input/InputActionReader")]
    public class InputActionReader : ScriptableObject, IInputActionReader
    {
        public event Action<Vector2> OnMoveChanged;
        public event Action<Vector2> OnLookChanged;
        public event Action<bool> OnAttackPhysicalChanged;
        public event Action<bool> OnAttackMagicalChanged;
        public event Action<bool> OnInteractChanged;
        public event Action<bool> OnCrouchChanged;
        public event Action<bool> OnJumpChanged;
        public event Action<bool> OnSprintChanged;
        public event Action OnPreviousTrigger;
        public event Action OnNextTrigger;

        public Vector2 MoveValue { get; private set; }
        public Vector2 LookValue { get; private set; }
        public bool IsAttackingPhysical { get; private set; }
        public bool IsAttackingMagical { get; private set; }
        public bool IsInteracting { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsSprinting { get; private set; }

        private PlayerInputSystem _playerInputSystem;

        private void OnEnable()
        {
            if (_playerInputSystem == null)
            {
                _playerInputSystem = new PlayerInputSystem();
                _playerInputSystem.Player.SetCallbacks(this);
            }

            _playerInputSystem?.Enable();
        }

        private void OnDisable()
        {
            _playerInputSystem?.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
            OnMoveChanged?.Invoke(MoveValue);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookValue = context.ReadValue<Vector2>();
            OnLookChanged?.Invoke(LookValue);
        }

        public void OnAttackPhysical(InputAction.CallbackContext context)
        {
            IsAttackingPhysical = context.ReadValueAsButton();
            OnAttackPhysicalChanged?.Invoke(IsAttackingPhysical);
        }

        public void OnAttackMagical(InputAction.CallbackContext context)
        {
            IsAttackingMagical = context.ReadValueAsButton();
            OnAttackMagicalChanged?.Invoke(IsAttackingMagical);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            IsInteracting = context.ReadValueAsButton();
            OnInteractChanged?.Invoke(IsInteracting);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            IsCrouching = context.ReadValueAsButton();
            OnCrouchChanged?.Invoke(IsCrouching);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            IsJumping = context.ReadValueAsButton();
            OnJumpChanged?.Invoke(IsJumping);
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            OnPreviousTrigger?.Invoke();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            OnNextTrigger?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            IsSprinting = context.ReadValueAsButton();
            OnSprintChanged?.Invoke(IsSprinting);
        }
    }
}