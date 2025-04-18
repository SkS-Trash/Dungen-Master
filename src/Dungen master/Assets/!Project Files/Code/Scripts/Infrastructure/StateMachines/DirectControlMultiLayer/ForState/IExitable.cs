using Cysharp.Threading.Tasks;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс для состояний, поддерживающих асинхронный выход.
    /// </summary>
    public interface IExitable
    {
        UniTask OnExitAsync();
    }
}