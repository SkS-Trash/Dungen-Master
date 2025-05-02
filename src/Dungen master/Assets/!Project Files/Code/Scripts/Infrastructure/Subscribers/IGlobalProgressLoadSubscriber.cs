using Progress;

/// <summary>
/// Интерфейс для подписчиков на загрузку глобального прогресса.
/// </summary>
public interface IGlobalProgressLoadSubscriber : IGlobalSubscriber
{
    /// <summary>
    /// Загрузить глобальный прогресс.
    /// </summary>
    /// <param name="progress"> Прогресс уровня.</param>
    void OnProgressLoaded(GlobalSaveData progress);
}