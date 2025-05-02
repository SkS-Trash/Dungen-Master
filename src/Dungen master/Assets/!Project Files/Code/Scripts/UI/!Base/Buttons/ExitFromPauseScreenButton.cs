namespace UI.Buttons
{
    public class ExitFromPauseScreenButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IExitFromPauseScreenSubscriber>(x => x.OnExitFromPauseHomeScreen());
        }
    }
}