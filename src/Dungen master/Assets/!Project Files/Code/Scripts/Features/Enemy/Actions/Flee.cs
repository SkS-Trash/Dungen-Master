using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Flee")]
    public class Flee : ActionSO
    {
        private Vector3 _vector3;

        public override void Act(StateController c)
        {
            _vector3 = (c.transform.position - c.Player.position).normalized;
            _vector3 = c.transform.position + _vector3 * c.Stats.fleeDistance;
            c.Movement.MoveTo(_vector3);
        }
    }
}