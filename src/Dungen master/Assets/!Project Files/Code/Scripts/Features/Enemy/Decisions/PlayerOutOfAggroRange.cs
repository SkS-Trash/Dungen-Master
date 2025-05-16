using Enemy.Core;
using UnityEngine;

namespace Enemy.Decisions
{
    [CreateAssetMenu(menuName = "AI/Decision/Player Out Of Aggro Range")]
    public class PlayerOutOfAggroRange : DecisionSO
    {
        public override bool Decide(StateController c) =>
            Vector3.Distance(c.transform.position, c.Player.position) > c.Stats.aggroRadius * 1.25f;
    }
}