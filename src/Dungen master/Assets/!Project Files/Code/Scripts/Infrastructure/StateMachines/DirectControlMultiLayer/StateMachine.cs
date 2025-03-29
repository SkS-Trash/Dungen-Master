using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using StateMachines.DirectControlMultiLayer.ForState;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Реализация контроллера "машины состояний".
    /// </summary>
    public sealed class StateMachine : IStateMachine
    {
        public event Action<IState> StateChanged;

        public Type CurrentStateType => _stateStack.Count > 0
            ? _stateStack.Peek().GetType()
            : null;

        private readonly IStatesFactory _statesFactory;
        private readonly Stack<IState> _stateStack = new();

        private CancellationTokenSource _updateLoopCts;

        public StateMachine(
            IStatesFactory statesFactory
        )
        {
            _statesFactory = statesFactory;
        }

        #region ChangeState

        public UniTask ChangeState<TState>() where TState : IState
            => SwitchStateInternal<TState, Unit>(Unit.Default, true);

        public UniTask ChangeState<TState, TArg>(TArg arg) where TState : IState
            => SwitchStateInternal<TState, TArg>(arg, true);

        #endregion

        #region PushState / PopState

        public UniTask PushState<TState>() where TState : IState
            => SwitchStateInternal<TState, Unit>(Unit.Default, false);

        public UniTask PushState<TState, TArg>(TArg arg) where TState : IState
            => SwitchStateInternal<TState, TArg>(arg, false);

        public async UniTask PopState()
        {
            if (_stateStack.Count == 0) return;

            await ExitCurrentStateAsync();
            _stateStack.Pop();

            if (_stateStack.Count > 0)
            {
                await EnterActiveStateAsync(Unit.Default);
            }
        }

        #endregion

        #region RunOneShot

        public async UniTask RunOneShot<TState>() where TState : IStateOneShot
        {
            var state = _statesFactory.CreateState<TState>();
            await state.OnEnterAsync(Unit.Default);
        }


        public async UniTask RunOneShot<TState, TArg>(TArg arg) where TState : IStateOneShot<TArg>
        {
            var state = _statesFactory.CreateState<TState>();
            await state.OnEnterAsync(arg);
        }

        #endregion

        #region Internal State Logic

        private async UniTask SwitchStateInternal<TState, TArg>(TArg arg, bool clearStack) where TState : IState
        {
            await ExitCurrentStateAsync();

            if (clearStack)
                _stateStack.Clear();

            var newState = ResolveState<TState>();
            _stateStack.Push(newState);

            await EnterActiveStateAsync(arg);
        }

        private async UniTask ExitCurrentStateAsync()
        {
            if (_stateStack.Count == 0) return;

            ExitCurrentIfUpdateableAsync();

            var currentState = _stateStack.Peek();
            if (currentState is IExitable exitable)
                await exitable.OnExitAsync();
        }

        private void ExitCurrentIfUpdateableAsync()
        {
            if (_stateStack.Count == 0) return;

            var currentState = _stateStack.Peek();
            if (currentState is IExecute)
            {
                _updateLoopCts?.Cancel();
                _updateLoopCts = null;
            }
        }

        private IState ResolveState<TState>() where TState : IState
        {
            var newState = _statesFactory.CreateState<TState>();

            return newState;
        }

        private async UniTask EnterActiveStateAsync<TArg>(TArg arg)
        {
            if (_stateStack.Count == 0) return;

            var activeState = _stateStack.Peek();

            StateChanged?.Invoke(activeState);

            if (activeState is IEnterable<Unit> enterable)
                await enterable.OnEnterAsync(Unit.Default);
            else if (activeState is IEnterable<TArg> enterableWithArg)
                await enterableWithArg.OnEnterAsync(arg);

            if (activeState is IExecute updateable)
            {
                _updateLoopCts = new CancellationTokenSource();
                RunUpdateLoopAsync(updateable, _updateLoopCts.Token).Forget();
            }
        }

        #endregion

        #region Update Loop

        private static async UniTaskVoid RunUpdateLoopAsync(IExecute execute, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await execute.OnExecuteAsync();

                    if (execute.TickIntervalMilliseconds > 0)
                    {
                        await UniTask.Delay(execute.TickIntervalMilliseconds, cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await UniTask.Yield(cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore
            }
        }

        #endregion
    }
}