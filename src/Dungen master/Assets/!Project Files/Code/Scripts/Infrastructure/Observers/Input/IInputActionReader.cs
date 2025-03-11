using System;
using UnityEngine;

namespace Infrastructure.Observers.Input
{
    public interface IInputActionReader
    {
        event Action<Vector2> OnMoveChange;
        event Action<Vector2> OnLookChange;
        event Action<bool> OnAttackPhysicalChange;
        event Action<bool> OnAttackMagicalChange;
        event Action<bool> OnInteractChange;
        event Action<bool> OnCrouchChange;
        event Action<bool> OnJumpChange;
        event Action OnPreviousTrigger;
        event Action OnNextTrigger;
        event Action<bool> OnSprintChange;

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