using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Face Player")]
    public class FacePlayer : ActionSO
    {
        public override void Act(StateController c)
        {
            var dir = c.Player.position - c.transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude < 0.01f) return;
            var rot = Quaternion.LookRotation(dir);
            c.transform.rotation = Quaternion.Slerp(c.transform.rotation, rot, Time.deltaTime * 10);
        }
    }
}