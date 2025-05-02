namespace UI.Buttons
{
    public class SettingsCloseButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<ISettingsCloseSubscriber>(x => x.CloseSettings());
        }
    }
}