public interface IPauseGameEvent : IGlobalSubscriber
{
    void OnPauseGame(bool isPaused);
}