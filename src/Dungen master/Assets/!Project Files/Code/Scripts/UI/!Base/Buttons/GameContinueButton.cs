using Progress;

namespace UI.Buttons
{
    public class GameContinueButton : ButtonView,
        IGlobalProgressLoadSubscriber
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            EventBus.Subscribe(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventBus.Unsubscribe(this);
        }

        protected override void OnClick()
        {
            EventBus.RaiseEvent<ILaunchContinueGame>(x => x.LaunchContinueGame());
        }

        public void OnProgressLoaded(GlobalSaveData progress)
        {
            if (progress.isFirstLaunch)
                InteractableOff();
            else
                InteractableOn();
        }
    }
}