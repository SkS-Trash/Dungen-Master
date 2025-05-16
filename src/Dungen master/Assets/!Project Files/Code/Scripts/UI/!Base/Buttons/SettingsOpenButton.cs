namespace UI.Buttons
{
    public class SettingsOpenButton : ButtonView
    {
        protected override void OnClick()
        {
            EventBus.RaiseEvent<IOpenSettingsEvent>(x => x.OnOpenSettings());
        }
    }
}