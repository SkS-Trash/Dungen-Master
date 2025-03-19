using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Services.Window
{
    /// <summary>
    /// Интерфейс для управления окнами в приложении.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Открывает окно с указанным идентификатором.
        /// </summary>
        /// <param name="windowID">Идентификатор окна.</param>
        /// <returns>Задача, представляющая асинхронную операцию открытия окна.</returns>
        UniTask Open(WindowID windowID);

        /// <summary>
        /// Открывает окно с указанным идентификатором и возвращает компонент типа T.
        /// </summary>
        /// <typeparam name="T">Тип компонента, который должен быть возвращен.</typeparam>
        /// <param name="windowID">Идентификатор окна.</param>
        /// <returns>Задача, представляющая асинхронную операцию открытия окна и получения компонента типа T.</returns>
        UniTask<T> OpenAndGet<T>(WindowID windowID) where T : Component;

        /// <summary>
        /// Возвращает компонент типа T из открытого окна с указанным идентификатором.
        /// </summary>
        /// <typeparam name="T">Тип компонента, который должен быть возвращен.</typeparam>
        /// <param name="windowID">Идентификатор окна.</param>
        /// <returns>Компонент типа T из открытого окна.</returns>
        T Get<T>(WindowID windowID) where T : Component;

        /// <summary>
        /// Закрывает окно с указанным идентификатором.
        /// </summary>
        /// <param name="windowID">Идентификатор окна.</param>
        void Close(WindowID windowID);

        /// <summary>
        /// Проверяет, открыто ли окно с указанным идентификатором.
        /// </summary>
        /// <param name="windowID">Идентификатор окна.</param>
        /// <returns>Истина, если окно открыто; иначе ложь.</returns>
        bool IsWindowOpened(WindowID windowID);
    }
}