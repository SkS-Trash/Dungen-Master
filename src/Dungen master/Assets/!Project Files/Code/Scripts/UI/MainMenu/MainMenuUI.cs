using Subscribers;
using Subscribers.EventBusSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUI : BaseUI
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button startNewGameButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            startNewGameButton.onClick.AddListener(StartGame);
            continueButton.onClick.AddListener(ContinueGame);
            exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            startNewGameButton.onClick.RemoveListener(StartGame);
            continueButton.onClick.RemoveListener(ContinueGame);
            exitButton.onClick.RemoveListener(Exit);
        }

        public void ContinueGameButtonInteractable(bool interactable)
        {
            continueButton.interactable = interactable;
        }

        private void ContinueGame() => EventBus.RaiseEvent<ILaunchNewGame>(x => x.LaunchNewGame());
        private void StartGame() => EventBus.RaiseEvent<ILaunchContinueGame>(x => x.LaunchContinueGame());
        private void Exit() => EventBus.RaiseEvent<IQuitApplication>(x => x.QuitApplication());
    }
}