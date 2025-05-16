public interface IPlayerMagicCooldownEvent : IGlobalSubscriber
{
    void OnPlayerMagicCooldownChanged(float percentage);
}