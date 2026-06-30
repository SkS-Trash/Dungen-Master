namespace UI.Buttons
{
    public class SettingsCloseButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<ISettingsCloseEvent>(x => x.OnCloseSettings());
        }
    }
}