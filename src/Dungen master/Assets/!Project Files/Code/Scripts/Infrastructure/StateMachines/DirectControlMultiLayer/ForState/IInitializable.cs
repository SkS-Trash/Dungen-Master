using Cysharp.Threading.Tasks;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс с методом инициализации состояния.
    /// </summary>
    public interface IInitializable
    {
        UniTask Initialize();
    }
}