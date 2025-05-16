using Cysharp.Threading.Tasks;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс для состояний, способных выполнять "OnEnter" без аргумента.
    /// При этом он сам наследует generic-вариант с типом Unit.
    /// </summary>
    public interface IEnterable : IEnterable<Unit>
    {
    }

    /// <summary>
    /// Интерфейс для состояний, поддерживающих асинхронный вход с аргументом.
    /// </summary>
    public interface IEnterable<TArg>
    {
        UniTask OnEnterAsync(TArg _);
    }
}