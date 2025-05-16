using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class OpenNewPanel : MonoBehaviour
    {
        public event Action OnPanelOpened;

        [SerializeField] private GameObject panelToClose;
        [SerializeField] private GameObject panelToOpen;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OpenPanel);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenPanel);
        }

        private void OpenPanel()
        {
            if (panelToOpen)
                panelToOpen.SetActive(true);

            if (panelToClose)
                panelToClose.SetActive(false);

            OnPanelOpened?.Invoke();
        }
    }
}