public interface IPauseGameSubscriber : IGlobalSubscriber
{
    void OnPauseGame(bool isPaused);
}