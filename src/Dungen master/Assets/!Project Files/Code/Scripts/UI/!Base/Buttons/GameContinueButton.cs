using Progress;

namespace UI.Buttons
{
    public class GameContinueButton : ButtonView,
        IGlobalProgressLoadEvent
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
            EventBus.RaiseEvent<ILaunchContinueGameEvent>(x => x.OnLaunchContinueGame());
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