using System.Collections;
using Subscribers;
using Subscribers.EventBusSystem;
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
            spell.Cast(magicCastPoint);

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
                EventBus.RaiseEvent<IPlayerMagicCooldownSubscriber>(x =>
                    x.OnPlayerMagicCooldownChanged(CurrentSpellCooldown(currentSpellCooldown)));

                yield return null;
            }
        }

        private float CurrentSpellCooldown(float currentSpellCooldown) => 
            1 - currentSpellCooldown / _spellCooldown;
    }
}