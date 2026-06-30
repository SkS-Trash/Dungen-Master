using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Boss Death Notification")]
    public class BossDeathNotification : ActionSO
    {
        public override void Act(StateController c) =>
            EventBus.RaiseEvent<IBossDeathEvent>(x => x.OnBossDeath());
    }
}