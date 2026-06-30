using UnityEngine;

namespace Services.CoroutineRunner
{
    /// <summary>
    /// Интерфейс для запуска и остановки корутин.
    /// </summary>
    public interface ICoroutineRunner
    {
        /// <summary>
        /// Запускает указанную корутину.
        /// </summary>
        /// <param name="routine">Корутин, который нужно запустить.</param>
        Coroutine StartCoroutine(System.Collections.IEnumerator routine);

        /// <summary>
        /// Останавливает указанную корутину.
        /// </summary>
        /// <param name="routine">Корутин, который нужно остановить.</param>
        void StopCoroutine(System.Collections.IEnumerator routine);
    }
}