using UnityEngine;

namespace Services.CursorControl
{
    /// <summary>
    /// Состояние курсора.
    /// </summary>
    public readonly struct CursorSnapshot
    {
        public readonly bool Visible;
        public readonly CursorLockMode LockMode;
        public readonly Texture2D Texture;
        public readonly Vector2 HotSpot;
        public readonly CursorMode CursorMode;

        public CursorSnapshot(
            bool visible,
            CursorLockMode lockMode,
            Texture2D texture,
            Vector2 hotSpot,
            CursorMode cursorMode
        )
        {
            Visible = visible;
            LockMode = lockMode;
            Texture = texture;
            HotSpot = hotSpot;
            CursorMode = cursorMode;
        }
    }
}