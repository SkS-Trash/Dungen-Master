using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Random Patrol")]
    public class RandomPatrol : ActionSO
    {
        private Vector3 _target;

        public override void Act(StateController c)
        {
            if (c.Movement.HasReachedDestination())
            {
                var pos = c.transform.position;
                _target = pos + (Random.insideUnitSphere * 4f);
                _target.y = pos.y;
                c.Movement.SetSpeed(c.Stats.moveSpeed * 0.6f);
                c.Movement.MoveTo(_target);
                c.Animator.SetIsWalk(true);
            }
        }
    }
}