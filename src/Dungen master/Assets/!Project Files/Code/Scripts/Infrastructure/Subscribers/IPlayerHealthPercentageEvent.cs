public interface IPlayerHealthPercentageEvent : IGlobalSubscriber
{
    void OnPlayerHealthPercentageChanged(float percentage);
}