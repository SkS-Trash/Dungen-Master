public interface IPlayerMagicCooldownSubscriber : IGlobalSubscriber
{
    void OnPlayerMagicCooldownChanged(float percentage);
}