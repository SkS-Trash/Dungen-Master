using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StateMachines.DirectControlMultiLayer
{
    /// <summary>
    /// Реализация контроллера "машины состояний".
    /// </summary>
    public class DirectControlMultiLayerStateMachine : IDirectControlMultiLayerStateMachine
    {
        /// <inheritdoc/>
        public event Action<IState> StateChanged;

        /// <inheritdoc/>
        public Type CurrentStateType => _stateStack.Count > 0
            ? _stateStack.Peek().GetType()
            : null;

        private readonly IStatesFactory _statesFactory;
        private readonly Stack<IState> _stateStack = new();

        private CancellationTokenSource _updateLoopCts;

        public DirectControlMultiLayerStateMachine(
            IStatesFactory statesFactory
        )
        {
            _statesFactory = statesFactory;
        }

        #region Public API

        /// <inheritdoc/>
        public async void ChangeState<TState>() where TState : IState
        {
            try
            {
                await ExitCurrentStateAsync();
                _stateStack.Clear();
                PushState<TState>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public async void ChangeState<TState, T0>(T0 arg) where TState : IState
        {
            try
            {
                await ExitCurrentStateAsync();
                _stateStack.Clear();
                PushState<TState, T0>(arg);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public async void PushState<TState>() where TState : IState
        {
            try
            {
                await ExitCurrentIfUpdateableAsync();

                var newState = ResolveState<TState>();
                _stateStack.Push(newState);

                await EnterActiveStateAsync(Unit.Default);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public async void PushState<TState, TArg>(TArg arg) where TState : IState
        {
            try
            {
                await ExitCurrentIfUpdateableAsync();

                var newState = ResolveState<TState>();
                _stateStack.Push(newState);

                await EnterActiveStateAsync(arg);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public async void PopState()
        {
            try
            {
                if (_stateStack.Count == 0) return;

                await ExitCurrentStateAsync();
                _stateStack.Pop();

                if (_stateStack.Count > 0)
                {
                    await EnterActiveStateAsync(Unit.Default);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public async UniTask RunWhileWaitingForCompletion<TState>() where TState : IState
        {
            try
            {
                var state = _statesFactory.CreateState<TState>();
                if (state is IEnterable<Unit> enterable)
                    await enterable.OnEnterAsync(Unit.Default);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }

            await UniTask.CompletedTask;
        }

        #endregion

        #region Internal State Logic

        private async UniTask ExitCurrentStateAsync()
        {
            if (_stateStack.Count == 0) return;

            await ExitCurrentIfUpdateableAsync();

            var currentState = _stateStack.Peek();
            if (currentState is IExitable exitable)
                await exitable.OnExitAsync();
        }

        private UniTask ExitCurrentIfUpdateableAsync()
        {
            if (_stateStack.Count == 0) return UniTask.CompletedTask;

            var currentState = _stateStack.Peek();
            if (currentState is IExecute)
            {
                _updateLoopCts?.Cancel();
                _updateLoopCts?.Dispose();
                _updateLoopCts = null;
            }

            return UniTask.CompletedTask;
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

            switch (activeState)
            {
                case IEnterable<Unit> enterable:
                    // Без аргумента
                    await enterable.OnEnterAsync(Unit.Default);
                    break;

                case IEnterable<TArg> enterableWithArg when arg is not null:
                    // С аргументом
                    await enterableWithArg.OnEnterAsync(arg);
                    break;

                case IEnterable<TArg> enterableDefault:
                    // Обработка кейса, если вдруг пришёл null, но состояние ждёт аргумента
                    await enterableDefault.OnEnterAsync(default);
                    break;
            }

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