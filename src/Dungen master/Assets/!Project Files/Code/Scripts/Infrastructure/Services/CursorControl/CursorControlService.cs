using System;
using UnityEngine;

namespace Services.CursorControl
{
    public class CursorControlService : ICursorControlService
    {
        public event Action<CursorSnapshot> OnChanged;

        public CursorLockMode LockMode => Cursor.lockState;
        public bool IsVisible => Cursor.visible;

        public Texture2D CurrentTexture { get; private set; }

        private CursorMode _currentMode;
        private Vector2 _currentHotSpot;

        public void SetVisible(bool visible)
        {
            if (Cursor.visible == visible) return;

            Cursor.visible = visible;
            if (visible && Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;

            FireChanged();
        }

        public void SetLock(CursorLockMode mode)
        {
            if (Cursor.lockState == mode) return;

            Cursor.lockState = mode;
            if (mode == CursorLockMode.Locked && Cursor.visible)
                Cursor.visible = false;

            FireChanged();
        }

        public void SetIcon(Texture2D texture, Vector2 hotSpot = default,
            CursorMode mode = CursorMode.Auto)
        {
            if (CurrentTexture == texture &&
                _currentHotSpot == hotSpot &&
                _currentMode == mode) return;

            CurrentTexture = texture;
            _currentHotSpot = hotSpot;
            _currentMode = mode;

            Cursor.SetCursor(texture, hotSpot, mode);
            FireChanged();
        }

        public CursorSnapshot Capture() =>
            new(Cursor.visible, Cursor.lockState, CurrentTexture, _currentHotSpot, _currentMode);

        public void Restore(CursorSnapshot s)
        {
            Cursor.visible = s.Visible;
            Cursor.lockState = s.LockMode;
            CurrentTexture = s.Texture;
            _currentHotSpot = s.HotSpot;
            _currentMode = s.CursorMode;
            Cursor.SetCursor(CurrentTexture, _currentHotSpot, _currentMode);

            FireChanged();
        }

        private void FireChanged() => OnChanged?.Invoke(Capture());
    }
}