namespace UI.Buttons
{
    public class ExitInHomeButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IExitInHomeSubscriber>(x => x.OnExitInHome());
        }
    }
}