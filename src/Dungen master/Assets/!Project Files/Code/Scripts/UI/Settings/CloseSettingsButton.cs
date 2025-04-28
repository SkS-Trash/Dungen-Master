using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    [RequireComponent(typeof(Button))]
    public class CloseSettingsButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(CloseSettings);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(CloseSettings);
        }

        private void CloseSettings()
        {
            EventBus.RaiseEvent<ICloseSettingsSubscriber>(x => x.CloseSettings());
        }
    }
}