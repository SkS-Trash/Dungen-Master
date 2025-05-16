using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/EffectDeathAction")]
    public class EffectDeath : ActionSO
    {
        public override void Act(StateController c)
        {
            c.GetComponent<Rigidbody>().isKinematic = true;
            var colliders = c.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
                collider.enabled = false;

            c.Animator.ApplyRootMotion(true);
            c.Animator.LaunchDeath();

            c.enabled = false;
        }
    }
}