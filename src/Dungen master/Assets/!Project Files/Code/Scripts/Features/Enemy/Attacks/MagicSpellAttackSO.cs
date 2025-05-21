using Enemy.Core;
using Magic;
using Magic.Data;
using UnityEngine;

namespace Enemy.Attacks
{
    [CreateAssetMenu(menuName = "AI/Attack/Magic")]
    public class MagicSpellAttackSO : EnemyAttackSO
    {
        [SerializeField] private Spell spell;

        public override void Execute(StateController controller)
        {
            var magic = controller.GetComponent<MagicCastController>();
            if (magic && magic.CanCast())
            {
                magic.SetSpell(spell);
                controller.Animator.LaunchAttack();
            }
        }
    }
}