using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class EnemyCurrentStateDraw : BaseUI
    {
        [SerializeField] private TMP_Text text;

        public void SetCurrentState(string state)
        {
            text.text = state;
        }
    }
}