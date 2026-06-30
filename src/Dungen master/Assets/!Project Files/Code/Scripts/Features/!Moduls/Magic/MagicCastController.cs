using System.Collections;
using Magic.Data;
using R3;
using UnityEngine;

namespace Magic
{
    public class MagicCastController : MonoBehaviour
    {
        public ReactiveProperty<float> SpellCooldown { get; } = new(1f);

        [SerializeField] private UnitType[] targetUnits = { UnitType.Player };
        [SerializeField] private Transform magicCastPoint;
        [SerializeField] private Spell spell;

        private float _spellCastToTime = 0;
        private float _spellCooldown = 0;

        public bool CanCast()
        {
            return _spellCooldown <= 0 || Time.time >= _spellCooldown + _spellCastToTime;
        }

        public void CastSpell(Spell spell = null)
        {
            var spawnPosition = magicCastPoint.position;
            var targetPosition = magicCastPoint.position + magicCastPoint.forward * 10;

            spell ??= this.spell;
            spell.Cast(targetUnits, spawnPosition, targetPosition);

            StartCoroutine(CooldownCoroutine());
        }

        public void SetSpell(Spell newSpell)
        {
            spell = newSpell;
        }

        private IEnumerator CooldownCoroutine()
        {
            _spellCastToTime = Time.time;
            _spellCooldown = spell.Cooldown;

            float currentSpellCooldown = 0;
            while (currentSpellCooldown < _spellCooldown)
            {
                currentSpellCooldown += Time.deltaTime;

                SpellCooldown.Value = currentSpellCooldown / _spellCooldown;

                yield return null;
            }
        }
    }
}