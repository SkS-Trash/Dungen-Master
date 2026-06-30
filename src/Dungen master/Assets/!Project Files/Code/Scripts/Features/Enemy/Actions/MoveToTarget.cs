using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Move To Player")]
    public class MoveToTarget : ActionSO
    {
        public override void Act(StateController c)
        {
            c.Movement.SetSpeed(c.Stats.CurrentValue.moveSpeed);
            c.Movement.MoveTo(c.Player.position);
            c.Animator.SetIsRun(true);
        }
    }
}