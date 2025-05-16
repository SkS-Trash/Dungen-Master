using Health;
using Progress;
using Services.Progress;
using Sirenix.OdinInspector;

namespace Enemy.Components
{
    public class EnemyHealth : HealthContainer,
        ILevelProgressLoadEvent, ILevelProgressCollector
    {
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

            WasDamaged = true;
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