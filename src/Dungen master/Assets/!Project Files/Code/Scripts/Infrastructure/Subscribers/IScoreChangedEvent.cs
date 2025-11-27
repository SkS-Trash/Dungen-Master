public interface IScoreChangedEvent : IGlobalSubscriber
{
    void OnScoreChanged(int value);
}