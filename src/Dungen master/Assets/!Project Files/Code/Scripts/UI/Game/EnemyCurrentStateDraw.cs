using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class EnemyCurrentStateDraw : ElementUI
    {
        [SerializeField] private TMP_Text text;

        public void SetCurrentState(string state)
        {
            text.text = state;
        }
    }
}