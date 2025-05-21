using System;
using UnityEngine;

namespace Observers.Input
{
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
}