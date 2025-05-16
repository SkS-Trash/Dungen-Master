namespace UI.Buttons
{
    public class QuitApplicationButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IQuitApplicationEvent>(x => x.OnQuitApplication());
        }
    }
}