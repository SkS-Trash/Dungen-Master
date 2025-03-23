using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUI : BaseUI
    {
        public event Action OnStartGame, OnExit;

        [SerializeField] private Button startGameButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            startGameButton.onClick.RemoveListener(StartGame);
            exitButton.onClick.RemoveListener(Exit);
        }

        private void StartGame() => OnStartGame?.Invoke();
        private void Exit() => OnExit?.Invoke();
    }
}