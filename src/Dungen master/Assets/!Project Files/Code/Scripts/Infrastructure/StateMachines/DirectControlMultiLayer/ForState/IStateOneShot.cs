namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Интерфейс для состояний, которые могут быть входными.
    /// </summary>
    public interface IStateOneShot : IState, IEnterable
    {
    }
    
    /// <summary>
    /// Интерфейс для состояний, которые могут быть входными.
    /// </summary>
    public interface IStateOneShot<TArg> : IState, IEnterable<TArg>
    {
    }
}