using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class ScoreUI : ElementUI,
        IScoreChangedEvent
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void OnEnable() => EventBus.Subscribe(this);
        private void OnDisable() => EventBus.Unsubscribe(this);

        public void SetPoints(int score)
        {
            scoreText.text = $"{score} Очков";
        }

        public void OnScoreChanged(int value) => SetPoints(value);
    }
}