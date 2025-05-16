using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Stop Moving")]
    public class StopMoving : ActionSO
    {
        public override void Act(StateController c)
        {
            c.Movement.StopMoving();
            c.Animator.SetIsRun(false);
            c.Animator.SetIsWalk(false);
        }
    }
}