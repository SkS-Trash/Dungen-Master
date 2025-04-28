using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class SettingsButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnSettingsClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnSettingsClicked);
        }

        private void OnSettingsClicked()
        {
            EventBus.RaiseEvent<IOpenSettingsSubscriber>(x => x.OpenSettings());
        }
    }
}