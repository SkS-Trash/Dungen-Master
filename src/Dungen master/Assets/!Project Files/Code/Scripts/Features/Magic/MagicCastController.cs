using UnityEngine;

namespace Magic
{
    public class MagicCastController : MonoBehaviour
    {
        [SerializeField] private Transform magicCastPoint;
        [SerializeField] private Spell spell;

        private float _spellCastToTime = 0;
        private float _spellCooldown = 0;

        public bool CanCast()
        {
            if (spell == null)
            {
                return false;
            }

            if (_spellCooldown > 0 && Time.time < _spellCooldown + _spellCastToTime)
            {
                return false;
            }

            return true;
        }

        public void CastSpell()
        {
            _spellCastToTime = Time.time;
            _spellCooldown = spell.Cooldown;

            spell.Cast(magicCastPoint);
        }

        public void SetSpell(Spell newSpell)
        {
            spell = newSpell;
        }
    }
}