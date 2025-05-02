namespace UI.Buttons
{
    public class ExitInMainMenuButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IExitInMainMenuSubscriber>(x => x.OnExitInMainMenu());
        }
    }
}