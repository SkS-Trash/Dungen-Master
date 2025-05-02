public interface IPlayerHealthPercentageSubscriber : IGlobalSubscriber
{
    void OnPlayerHealthPercentageChanged(float percentage);
}