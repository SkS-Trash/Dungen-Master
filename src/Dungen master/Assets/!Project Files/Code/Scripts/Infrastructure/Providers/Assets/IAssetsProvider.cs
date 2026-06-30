using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Providers.Assets
{
    /// <summary>
    /// Интерфейс для провайдера ассетов.
    /// </summary>
    public interface IAssetsProvider
    {
        /// <summary>
        /// Получает ассет по адресу.
        /// </summary>
        /// <typeparam name="T">Тип ассета.</typeparam>
        /// <param name="address">Адрес ассета.</param>
        /// <returns>Асинхронная задача, возвращающая ассет типа T.</returns>
        UniTask<T> GetAsset<T>(string address) where T : Object;

        /// <summary>
        /// Получает ассет по ссылке на ассет.
        /// </summary>
        /// <typeparam name="T">Тип ассета.</typeparam>
        /// <param name="assetReference">Ссылка на ассет.</param>
        /// <returns>Асинхронная задача, возвращающая ассет типа T.</returns>
        UniTask<T> GetAsset<T>(AssetReference assetReference) where T : Object;

        /// <summary>
        /// Получает список ассетов по адресам.
        /// </summary>
        /// <typeparam name="T">Тип ассетов.</typeparam>
        /// <param name="addresses">Коллекция адресов ассетов.</param>
        /// <returns>Асинхронная задача, возвращающая список ассетов типа T.</returns>
        UniTask<List<T>> GetAssets<T>(IEnumerable<string> addresses) where T : Object;

        /// <summary>
        /// Очищает ресурсы, используемые провайдером ассетов.
        /// </summary>
        void CleanUp();
    }
}