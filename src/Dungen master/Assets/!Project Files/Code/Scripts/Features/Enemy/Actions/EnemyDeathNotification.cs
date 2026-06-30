using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Enemy Death Notification")]
    public class EnemyDeathNotification : ActionSO
    {
        public override void Act(StateController c) =>
            EventBus.RaiseEvent<IEnemyDeathEvent>(x => x.OnEnemyDeath());
    }
}