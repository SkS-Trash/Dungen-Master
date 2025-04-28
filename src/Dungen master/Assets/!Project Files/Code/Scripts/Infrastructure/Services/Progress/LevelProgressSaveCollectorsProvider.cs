using System.Collections.Generic;
using Progress;

namespace Services.Progress
{
    /// <summary>
    /// Провайдер-агрегатор подписчиков на сохранение/загрузку прогресса уровня.
    /// </summary>
    public class LevelProgressSaveCollectorsProvider
    {
        public static LevelProgressSaveCollectorsProvider Instance { get; } = new();

        private readonly List<ILevelProgressCollector> _collectors = new();

        public void Collect(LevelSaveData target)
        {
            _collectors.RemoveAll(c => c == null);
            foreach (var collector in _collectors)
            {
                collector.Collect(target);
            }
        }

        public void Subscribe(ILevelProgressCollector collector)
        {
            if (!_collectors.Contains(collector))
            {
                _collectors.Add(collector);
            }
            else throw new System.Exception("Подписчик уже подписан на событие сохранения прогресса.");
        }

        public void Unsubscribe(ILevelProgressCollector collector)
        {
            _collectors.Remove(collector);
        }
    }
}