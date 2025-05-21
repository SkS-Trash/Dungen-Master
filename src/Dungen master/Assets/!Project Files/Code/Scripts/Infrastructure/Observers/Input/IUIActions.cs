using System;
using UnityEngine;

namespace Observers.Input
{
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