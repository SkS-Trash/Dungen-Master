namespace Providers
{
    /// <summary>
    /// Интерфейс для предоставления статических данных.
    /// </summary>
    public interface IStaticDataProvider
    {
        ProgressGameDataHolder GetProgressGameDataHolder();
    }
}