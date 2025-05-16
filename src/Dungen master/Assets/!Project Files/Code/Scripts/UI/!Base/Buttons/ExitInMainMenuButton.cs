namespace UI.Buttons
{
    public class ExitInMainMenuButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IExitInMainMenuEvent>(x => x.OnExitInMainMenu());
        }
    }
}