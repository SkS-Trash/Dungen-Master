using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Observers.Input
{
    [CreateAssetMenu(menuName = "Data/Input/InputActionReader")]
    public class InputActionReader : ScriptableObject, IInputActionReader,
        PlayerInputSystem.IPlayerActions,
        PlayerInputSystem.IUIActions
    {
        private PlayerInputSystem _playerInputSystem;

        private void OnEnable()
        {
            if (_playerInputSystem == null)
            {
                _playerInputSystem = new PlayerInputSystem();
                _playerInputSystem.Player.SetCallbacks(this);
                _playerInputSystem.UI.SetCallbacks(this);
            }

            _playerInputSystem?.Enable();
        }

        private void OnDisable()
        {
            _playerInputSystem?.Disable();
        }

        #region Player Input System

        public event Action<Vector2> OnMoveChanged;
        public event Action<Vector2> OnLookChanged;
        public event Action OnAttackPhysicalChanged;
        public event Action OnAttackMagicalChanged;
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
            OnAttackPhysicalChanged?.Invoke();
        }

        public void OnAttackMagical(InputAction.CallbackContext context)
        {
            IsAttackingMagical = context.ReadValueAsButton();
            OnAttackMagicalChanged?.Invoke();
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
            if (context.performed)
                OnPreviousTrigger?.Invoke();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnNextTrigger?.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            IsSprinting = context.ReadValueAsButton();
            OnSprintChanged?.Invoke(IsSprinting);
        }

        #endregion

        #region UI Input System

        // UI actions
        public event Action<Vector2> OnNavigateChanged;
        public event Action<Vector2> OnPointChanged;
        public event Action<Vector2> OnScrollWheelChanged;
        public event Action OnSubmitChanged;
        public event Action OnCancelChanged;
        public event Action<bool> OnClickChanged;
        public event Action<bool> OnRightClickChanged;
        public event Action<bool> OnMiddleClickChanged;

        // UI Input values
        public Vector2 NavigateValue { get; private set; }
        public Vector2 PointValue { get; private set; }
        public Vector2 ScrollWheelValue { get; private set; }
        public bool IsSubmitting { get; private set; }
        public bool IsCanceling { get; private set; }
        public bool IsClicking { get; private set; }
        public bool IsRightClicking { get; private set; }
        public bool IsMiddleClicking { get; private set; }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            NavigateValue = context.ReadValue<Vector2>();
            OnNavigateChanged?.Invoke(NavigateValue);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            IsSubmitting = context.ReadValueAsButton();

            if (context.performed)
                OnSubmitChanged?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            IsCanceling = context.ReadValueAsButton();
            
            if (context.performed)
                OnCancelChanged?.Invoke();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            PointValue = context.ReadValue<Vector2>();
            OnPointChanged?.Invoke(PointValue);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            IsClicking = context.ReadValueAsButton();
            OnClickChanged?.Invoke(IsClicking);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            IsRightClicking = context.ReadValueAsButton();
            OnRightClickChanged?.Invoke(IsRightClicking);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            IsMiddleClicking = context.ReadValueAsButton();
            OnMiddleClickChanged?.Invoke(IsMiddleClicking);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            ScrollWheelValue = context.ReadValue<Vector2>();
            OnScrollWheelChanged?.Invoke(ScrollWheelValue);
        }

        #endregion
    }
}