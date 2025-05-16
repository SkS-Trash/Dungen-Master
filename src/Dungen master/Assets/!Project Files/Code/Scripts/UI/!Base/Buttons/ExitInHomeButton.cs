namespace UI.Buttons
{
    public class ExitInHomeButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IExitInHomeEvent>(x => x.OnExitInHome());
        }
    }
}