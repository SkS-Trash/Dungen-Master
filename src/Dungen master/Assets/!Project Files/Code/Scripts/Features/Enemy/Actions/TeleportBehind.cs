using Enemy.Core;
using UnityEngine;

namespace Enemy.Actions
{
    [CreateAssetMenu(menuName = "AI/Action/Teleport Behind Target")]
    public class TeleportBehind : ActionSO
    {
        [SerializeField] private float distance = 1.5f;

        public override void Act(StateController c)
        {
            var p = c.Player.position - c.Player.forward * distance;
            p.y = c.transform.position.y;
            c.transform.position = p;
        }
    }
}