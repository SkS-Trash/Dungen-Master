using Cysharp.Threading.Tasks;

namespace StateMachines.DirectControlMultiLayer.ForState
{
    /// <summary>
    /// Интерфейс для состояний, поддерживающих асинхронный выход.
    /// </summary>
    public interface IExitable
    {
        UniTask OnExitAsync();
    }
}