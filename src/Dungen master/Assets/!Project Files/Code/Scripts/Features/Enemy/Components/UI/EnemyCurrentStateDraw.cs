using TMPro;
using UI;
using UnityEngine;

namespace Enemy.Components.UI
{
    public class EnemyCurrentStateDraw : ElementUI
    {
        [SerializeField] private TMP_Text text;

        protected override void Awake()
        {
            base.Awake();
#if !UNITY_EDITOR
            text.gameObject.SetActive(false);
#endif
        }

        public void SetStateText(string state)
        {
#if UNITY_EDITOR
            text.text = state;
#endif
        }
    }
}