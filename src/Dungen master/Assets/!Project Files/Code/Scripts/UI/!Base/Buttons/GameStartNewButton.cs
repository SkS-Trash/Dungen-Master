namespace UI.Buttons
{
    public class GameStartNewButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<ILaunchNewGameEvent>(x => x.OnLaunchNewGame());
        }
    }
}