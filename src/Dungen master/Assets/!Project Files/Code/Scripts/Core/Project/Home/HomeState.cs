using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer;

namespace Core.Project.Home
{
    public class HomeState : IState, IEnterable, IExitable
    {
        public UniTask OnEnterAsync(Unit _)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnExitAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}