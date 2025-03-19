using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines.DirectControlMultiLayer.ForState
{
    /// <summary>
    /// Интерфейс для состояний, поддерживающих периодический вызов метода OnExecuteAsync.
    /// </summary>
    public interface IExecute
    {
        /// <summary>
        /// Интервал (мс) между вызовами OnUpdateAsync.
        /// </summary>
        int TickIntervalMilliseconds { get; }

        /// <summary>
        /// Асинхронный метод, вызываемый раз в TickIntervalMilliseconds.
        /// </summary>
        UniTask OnExecuteAsync();
    }
}