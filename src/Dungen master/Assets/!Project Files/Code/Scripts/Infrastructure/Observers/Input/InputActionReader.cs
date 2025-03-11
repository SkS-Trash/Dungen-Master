using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Observers.Input
{
    [CreateAssetMenu(menuName = "Data/Input/InputActionReader")]
    public class InputActionReader : ScriptableObject, IInputActionReader, PlayerInputSystem.IPlayerActions
    {
        public event Action<Vector2> OnMoveChange;
        public event Action<Vector2> OnLookChange;
        public event Action<bool> OnAttackPhysicalChange;
        public event Action<bool> OnAttackMagicalChange;
        public event Action<bool> OnInteractChange;
        public event Action<bool> OnCrouchChange;
        public event Action<bool> OnJumpChange;
        public event Action OnPreviousTrigger;
        public event Action OnNextTrigger;
        public event Action<bool> OnSprintChange;

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
            OnMoveChange?.Invoke(MoveValue);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookValue = context.ReadValue<Vector2>();
            OnLookChange?.Invoke(LookValue);
        }

        public void OnAttackPhysical(InputAction.CallbackContext context)
        {
            IsAttackingPhysical = context.ReadValueAsButton();
            OnAttackPhysicalChange?.Invoke(IsAttackingPhysical);
        }

        public void OnAttackMagical(InputAction.CallbackContext context)
        {
            IsAttackingMagical = context.ReadValueAsButton();
            OnAttackMagicalChange?.Invoke(IsAttackingMagical);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            IsInteracting = context.ReadValueAsButton();
            OnInteractChange?.Invoke(IsInteracting);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            IsCrouching = context.ReadValueAsButton();
            OnCrouchChange?.Invoke(IsCrouching);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            IsJumping = context.ReadValueAsButton();
            OnJumpChange?.Invoke(IsJumping);
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
            OnSprintChange?.Invoke(IsSprinting);
        }
    }
}