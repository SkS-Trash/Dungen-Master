using System;
using UnityEngine;

namespace Observers.Input
{
    public interface IInputActionReader :
        IPlayerInputActions,
        IUIActions
    {
    }

    public interface IPlayerInputActions
    {
        event Action<Vector2> OnMoveChanged;
        event Action<Vector2> OnLookChanged;
        event Action<bool> OnInteractChanged;
        event Action<bool> OnCrouchChanged;
        event Action<bool> OnJumpChanged;
        event Action<bool> OnSprintChanged;
        event Action OnAttackPhysicalChanged;
        event Action OnAttackMagicalChanged;
        event Action OnPreviousTrigger;
        event Action OnNextTrigger;

        Vector2 MoveValue { get; }
        Vector2 LookValue { get; }
        bool IsAttackingPhysical { get; }
        bool IsAttackingMagical { get; }
        bool IsInteracting { get; }
        bool IsCrouching { get; }
        bool IsJumping { get; }
        bool IsSprinting { get; }
    }

    public interface IUIActions
    {
        event Action<Vector2> OnNavigateChanged;
        event Action<Vector2> OnPointChanged;
        event Action<Vector2> OnScrollWheelChanged;
        event Action<bool> OnClickChanged;
        event Action<bool> OnRightClickChanged;
        event Action<bool> OnMiddleClickChanged;
        event Action OnCancelChanged;
        event Action OnSubmitChanged;

        Vector2 NavigateValue { get; }
        Vector2 PointValue { get; }
        Vector2 ScrollWheelValue { get; }
        bool IsSubmitting { get; }
        bool IsCanceling { get; }
        bool IsClicking { get; }
        bool IsRightClicking { get; }
        bool IsMiddleClicking { get; }
    }
}