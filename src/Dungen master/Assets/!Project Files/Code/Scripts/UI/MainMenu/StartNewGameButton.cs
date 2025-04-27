using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class StartNewGameButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnStartNewGameButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnStartNewGameButtonClicked);
        }

        private void OnStartNewGameButtonClicked()
        {
            EventBus.RaiseEvent<ILaunchNewGame>(x => x.LaunchNewGame());
        }
    }
}