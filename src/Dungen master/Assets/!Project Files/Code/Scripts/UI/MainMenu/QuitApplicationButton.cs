using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class QuitApplicationButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnQuitApplicationClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnQuitApplicationClicked);
        }

        private void OnQuitApplicationClicked()
        {
            EventBus.RaiseEvent<IQuitApplication>(x => x.QuitApplication());
        }
    }
}