using Progress;
using Services.Progress;
using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class ContinueGameButton : MonoBehaviour,
        IGlobalProgressLoadSubscriber
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnContinueGameButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnContinueGameButtonClicked);
        }

        private void OnContinueGameButtonClicked()
        {
            EventBus.RaiseEvent<ILaunchContinueGame>(x => x.LaunchContinueGame());
        }

        public void OnProgressLoaded(GlobalSaveData progress)
        {
            _button.interactable = !progress.isFirstLaunch;
        }
    }
}