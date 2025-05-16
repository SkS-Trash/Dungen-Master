using System;
using UnityEngine;

namespace Services.CursorControl
{
    public interface ICursorControlService
    {
        bool IsVisible { get; }
        CursorLockMode LockMode { get; }
        Texture2D CurrentTexture { get; }

        /// <summary>Включить/выключить курсор (visible + unlock).</summary>
        void SetVisible(bool visible);

        /// <summary>Заблокировать/разблокировать курсор.</summary>
        void SetLock(CursorLockMode mode);

        /// <summary>Сменить спрайт. Передай null, чтобы вернуть системный.</summary>
        void SetIcon(Texture2D texture, Vector2 hotSpot = default, CursorMode mode = CursorMode.Auto);

        /// <summary>Сделать снимок состояния (для отката).</summary>
        CursorSnapshot Capture();

        /// <summary>Катастрофический откат к снимку.</summary>
        void Restore(CursorSnapshot snapshot);

        /// <summary>Событие на любое изменение.</summary>
        event Action<CursorSnapshot> OnChanged;
    }
}