using System.Threading.Tasks;
using Services.Window;
using UnityEngine;

namespace Factories.UI
{
    /// <summary>
    /// Интерфейс для фабрики UI.
    /// </summary>
    public interface IUIFactory
    {
        /// <summary>
        /// Создает экран на основе указанного адреса ресурса и идентификатора окна.
        /// </summary>
        /// <param name="assetAddress">Адрес ресурса экрана.</param>
        /// <param name="windowId">Идентификатор окна.</param>
        /// <returns>Задача, возвращающая созданный объект GameObject.</returns>
        Task<UnityEngine.GameObject> CreateScreen(string assetAddress, WindowID windowId);

        /// <summary>
        /// Получает компонент экрана указанного типа по идентификатору окна.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="windowId">Идентификатор окна.</param>
        /// <returns>Компонент экрана указанного типа.</returns>
        T GetScreenComponent<T>(WindowID windowId) where T : Component;

        /// <summary>
        /// Уничтожает экран по идентификатору окна.
        /// </summary>
        /// <param name="windowId">Идентификатор окна.</param>
        void DestroyScreen(WindowID windowId);

        /// <summary>
        /// Проверяет существование экрана по идентификатору окна.
        /// </summary>
        /// <param name="windowId">Идентификатор окна.</param>
        /// <returns>Истина, если экран существует; иначе ложь.</returns>
        bool Exists(WindowID windowId);
    }
}