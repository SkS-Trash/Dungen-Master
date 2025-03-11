namespace Services
{
    /// <summary>
    /// Интерфейс для сервиса сохранения и загрузки данных.
    /// </summary>
    public interface ISaveLoadDataService
    {
        /// <summary>
        /// Сохраняет данные по указанному ключу.
        /// </summary>
        /// <typeparam name="T">Тип данных для сохранения.</typeparam>
        /// <param name="data">Данные для сохранения.</param>
        /// <param name="key">Ключ для сохранения данных.</param>
        void Save<T>(T data, string key);

        /// <summary>
        /// Загружает данные по указанному ключу.
        /// </summary>
        /// <typeparam name="T">Тип данных для загрузки.</typeparam>
        /// <param name="key">Ключ для загрузки данных.</param>
        /// <returns>Загруженные данные.</returns>
        T Load<T>(string key);
    }
}