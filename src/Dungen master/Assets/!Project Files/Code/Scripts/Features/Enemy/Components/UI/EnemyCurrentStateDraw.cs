using Enemy.Core;
using TMPro;
using UI;
using UnityEngine;

namespace Enemy.Components.UI
{
    public class EnemyCurrentStateDraw : ElementUI
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private StateController stateController;
        
#if !UNITY_EDITOR
        protected override void Awake()
        {
            base.Awake();
            text.gameObject.SetActive(false);
        }
#endif


#if UNITY_EDITOR
        private void Update()
        {
            text.text = stateController.currentState.name;
        }
#endif
    }
}