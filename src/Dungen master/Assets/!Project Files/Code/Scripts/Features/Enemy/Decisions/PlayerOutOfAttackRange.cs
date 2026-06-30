using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Player Out Of Attack Range")]
    public class PlayerOutOfAttackRange : DecisionSO
    {
        public override bool Decide(StateController c) =>
            Vector3.Distance(c.transform.position, c.Player.position) > c.Stats.CurrentValue.attackRadius * 1.25f;
    }
}