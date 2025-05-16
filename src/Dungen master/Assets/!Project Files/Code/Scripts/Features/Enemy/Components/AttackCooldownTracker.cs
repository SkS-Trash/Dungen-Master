using System.Collections.Generic;
using Enemy.Core;
using UnityEngine;

namespace Enemy.Components
{
    public class AttackCooldownTracker : MonoBehaviour
    {
        private readonly Dictionary<EnemyAttackSO, float> _cooldowns = new();

        public bool IsReady(EnemyAttackSO attack)
        {
            return !_cooldowns.TryGetValue(attack, out var readyAt) || Time.time >= readyAt;
        }

        public void SetCooldown(EnemyAttackSO attack)
        {
            _cooldowns[attack] = Time.time + attack.cooldown;
        }

        public void ResetCooldown()
        {
            var attacks = new HashSet<EnemyAttackSO>(_cooldowns.Keys);
            foreach (var attack in attacks)
            {
                _cooldowns[attack] = Time.time;
            }
        }
    }
}