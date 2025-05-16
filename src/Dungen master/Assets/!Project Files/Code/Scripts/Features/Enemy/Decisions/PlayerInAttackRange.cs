using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Player In Attack Range")]
    public class PlayerInAttackRange : DecisionSO
    {
        public override bool Decide(StateController c) =>
            Vector3.Distance(c.transform.position, c.Player.position) < c.Stats.attackRadius;
    }
}