using System;
using Health;
using Progress;
using Services.Progress;
using Sirenix.OdinInspector;
using Subscribers.EventBusSystem;

namespace Enemy
{
    public class EnemyHealth : HealthContainer,
        ILevelProgressLoadSubscriber, ILevelProgressCollector
    {
        public event Action<int> OnHealthChanged;

        [field: ShowInInspector, HideInEditorMode]
        public bool WasDamaged { get; set; }

        private void OnEnable()
        {
            EventBus.Subscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
            LevelProgressSaveCollectorsProvider.Instance.Unsubscribe(this);
        }

        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);

            OnHealthChanged?.Invoke(CurrentHealth);

            WasDamaged = true;
        }

        public override void Heal(int amount)
        {
            base.Heal(amount);

            OnHealthChanged?.Invoke(CurrentHealth);
        }

        public void OnProgressLoaded(LevelSaveData progress)
        {
            var enemy = progress.enemies.Find(x => x.guid == gameObject.name);
            if (enemy == null)
            {
                return;
            }

            CurrentHealth = enemy.health;
        }

        public void Collect(LevelSaveData target)
        {
            var enemy = target.enemies.Find(x => x.guid == gameObject.name);
            if (enemy == null)
            {
                enemy = new EnemyData
                {
                    guid = gameObject.name
                };
                target.enemies.Add(enemy);
            }

            enemy.health = CurrentHealth;
        }
    }
}