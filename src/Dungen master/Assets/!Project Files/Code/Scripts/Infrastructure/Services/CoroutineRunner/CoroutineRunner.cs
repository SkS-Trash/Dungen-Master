using UnityEngine;

namespace Services
{
    /// <summary>
    /// Реализация интерфейса <see cref="ICoroutineRunner"/> для запуска и остановки корутин.
    /// </summary>
    public class CoroutineRunner : ICoroutineRunner
    {
        private readonly RunnerBehaviour _runner;

        public CoroutineRunner()
        {
            _runner = new GameObject("CoroutineRunner").AddComponent<RunnerBehaviour>();

            Object.DontDestroyOnLoad(_runner.gameObject);

            _runner.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        /// <inheritdoc/>
        public Coroutine StartCoroutine(System.Collections.IEnumerator routine)
        {
            return _runner.StartCoroutine(routine);
        }

        /// <inheritdoc/>
        public void StopCoroutine(System.Collections.IEnumerator routine)
        {
            _runner.StopCoroutine(routine);
        }
    }
}