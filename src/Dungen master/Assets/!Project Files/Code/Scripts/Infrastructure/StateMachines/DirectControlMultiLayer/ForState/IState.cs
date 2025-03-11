using System;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Общий интерфейс для состояний.
    /// </summary>
    public interface IState : IInitializable, IDisposable
    {
        /// <summary>
        /// Флаг, говорит нужно ли переиспользовать это состояние (true),
        /// либо при следующем вызове создаём новое (false).
        /// </summary>
        bool IsReusable { get; }
    }
}